
using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Farms;
using Business.Repository;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.Implements.Producers.Farms;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Producers.Farms;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Select;
using Entity.DTOs.Producer.Farm.Update;
using Entity.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities.Custom.Code;
using Utilities.Exceptions;
using Utilities.Helpers.Business;
using Utilities.QR.Interfaces;

namespace Business.Services.Producers.Farms
{
    public class FarmService : BusinessGeneric<FarmRegisterDto, FarmSelectDto, Farm>, IFarmService
    {

        private readonly IFarmRepository _farmRepository;
        private readonly IFarmImageRepository _farmImageRepository;
        private readonly IRolUserRepository _rolUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<FarmService> _logger;
        private readonly IQrCodeService _qr;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        private const int MaxImages = 5;

        public FarmService(IDataGeneric<Farm> data,
                           IMapper mapper,
                           IFarmRepository farmRepository,
                           IRolUserRepository rolUserRepository,
                           IUserRepository userRepository,
                           IProducerRepository producerRepository,
                           ICloudinaryService cloudinaryService,
                           ILogger<FarmService> logger,
                           IFarmImageRepository imageRepository,
                           ApplicationDbContext context,
                           IQrCodeService qr,
                           IConfiguration configuration
                            ) : base(data, mapper)
        {
            _farmRepository = farmRepository;
            _rolUserRepository = rolUserRepository;
            _userRepository = userRepository;
            _producerRepository = producerRepository;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
            _farmImageRepository = imageRepository;
            _context = context;
            _qr = qr;
            _configuration = configuration;
        }

        public override async Task<IEnumerable<FarmSelectDto>> GetAllAsync()
        {
            try
            {
                var entities = await _farmRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<FarmSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de fincas.", ex);
            }
        }

        public override async Task<FarmSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var entity = await _farmRepository.GetByIdAsync(id);
                return entity == null ? default : _mapper.Map<FarmSelectDto>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener la finca con ID {id}.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var entity = await _farmRepository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Intento de eliminar un producto inexistente con ID {Id}", id);
                return false;
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var images = await _farmImageRepository.GetByFarmIdAsync(id);

                foreach (var image in images)
                {
                    await _cloudinaryService.DeleteAsync(image.PublicId);
                    await _farmImageRepository.DeleteLogicalByPublicIdAsync(image.PublicId);
                }
                await _context.SaveChangesAsync();

                var deleted = await _farmRepository.DeleteLogicAsync(id);
                await _context.SaveChangesAsync();

                if (deleted)
                    _logger.LogInformation("Finca eliminada con ID {Id}", id);
                else
                    _logger.LogError("Error al eliminar la finca con ID {Id}", id);

                await transaction.CommitAsync();

                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando la finca ID {Id}", id);
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<FarmSelectDto> RegisterWithProducer(ProducerWithFarmRegisterDto dto, int userId)
        {
            // 0) Validaciones iniciales
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new BusinessException("Usuario no encontrado");

            if (user.Producer != null)
                throw new BusinessException("El usuario ya es productor");

            // Reutiliza la misma regla que en CreateFarmAsync
            ValidateMaxImages(dto.Images?.Count ?? 0);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1) Crear Productor
        
                var producer = dto.Adapt<Producer>();
                producer.Id = 0;                    
                producer.UserId = user.Id;           
                producer.User = null;  
                producer.Code = CodeGenerator.Generate(10);

                await _producerRepository.AddAsync(producer);
                await _context.SaveChangesAsync();   

                // 1.1) Crear redes sociales
                if (dto.SocialLinks != null && dto.SocialLinks.Count > 0)
                {

                    var duplicated = dto.SocialLinks
                        .GroupBy(x => x.Network)
                        .FirstOrDefault(g => g.Count() > 1);
                    if (duplicated != null)
                        throw new BusinessException($"Red social duplicada: {duplicated.Key}");

                    // Normaliza y crea entidades
                    var links = dto.SocialLinks
                        .Select(sl =>
                        {
                            var url = Urls.NormalizeUrl(sl.Network, sl.Url);
                            return new ProducerSocialLink
                            {
                                ProducerId = producer.Id,
                                Network = sl.Network,
                                Url = url
                            };
                        })
                        .ToList();


                    await _context.Set<ProducerSocialLink>().AddRangeAsync(links);
                    await _context.SaveChangesAsync();
                }

                // 2) Asignar rol de productor (dentro de la misma transacción)
                await _rolUserRepository.AsignateRolProducer(user);
                await _context.SaveChangesAsync();

                // 3) Crear Finca (sin imágenes todavía)
                var farm = dto.Adapt<Farm>();
                farm.Id = 0;                       
                farm.ProducerId = producer.Id;

                await _farmRepository.AddAsync(farm);
                await _context.SaveChangesAsync();  

                // 4) Subir y mapear imágenes reutilizando tu rutina nueva
                var images = await UploadAndMapImagesAsync(dto.Images, farm.Id);
                if (images.Any())
                {
                    await _farmImageRepository.AddImages(images);
                    await _context.SaveChangesAsync();
                }

                // 5) Commit
                await transaction.CommitAsync();

                // 5.1) === QR: generar PNG con QRCoder
                try
                {
                    var baseUrl = (_configuration["PublicBaseUrl"] ?? string.Empty).TrimEnd('/');
                    if (string.IsNullOrWhiteSpace(baseUrl))
                    {
                        _logger.LogWarning("PublicBaseUrl no configurado. No se generará QR para productor {ProducerId}", producer.Id);
                    }
                    else
                    {
                        var qrTargetUrl = $"{baseUrl}/home/product/profile/{producer.Code}";
                        var pngBytes = _qr.GeneratePng(qrTargetUrl);

                        var folder = $"producers/{producer.Id}";
                        var upload = await _cloudinaryService.UploadBytesAsync(
                            data: pngBytes,
                            folder: folder,
                            publicId: "qr_png",                       
                            fileNameWithExtension: $"qr_{producer.Code}.png",
                            contentType: "image/png",
                            overwrite: true
                        );

                        producer.QrUrl = upload.SecureUrl?.AbsoluteUri;
                        await _producerRepository.UpdateAsync(producer);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Falló generación/subida de QR para productor {ProducerId}", producer.Id);
                }


                // 6) DTO de salida consistente con CreateFarmAsync
                var result = farm.Adapt<FarmSelectDto>();
                result.Images = images.Adapt<List<FarmImageSelectDto>>();
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error registrando productor y finca");
                throw new BusinessException("Error al registrar la finca con el productor", ex);
            }
        }

        public async Task<FarmRegisterDto> CreateFarmAsync(FarmRegisterDto dto)
        {
            ValidateMaxImages(dto.Images?.Count ?? 0);
            var pid = await _producerRepository.GetIdProducer(dto.ProducerId)
                     ?? throw new BusinessException("El usuario no está registrado como productor.");
            var entity = dto.Adapt<Farm>();
            entity.ProducerId = pid;

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                await _farmRepository.AddAsync(entity);
                await _context.SaveChangesAsync();

                var images = await UploadAndMapImagesAsync(dto.Images, entity.Id);
                if (images.Any())
                {
                    await _farmImageRepository.AddImages(images);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                var result = entity.Adapt<FarmRegisterDto>();
                //result.Images = images.Adapt<List<FarmImageSelectDto>>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando producto");
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<FarmSelectDto>> GetByProducer(int userId)
        {
            try
            {
                var producerId = await _producerRepository.GetIdProducer(userId);
                if (producerId == null)
                    throw new BusinessException("El usuario no está registrado como productor.");
                var entities = await _farmRepository.GetByProducer(producerId);
                return _mapper.Map<IEnumerable<FarmSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de fincas del productor {producerId}.", ex);
            }
        }

        public async Task<IEnumerable<FarmSelectDto>> GetByProducerCodeAsync(string codeProducer)
        {
            try
            {
               
                var entities = await _farmRepository.GetByProducerCode(codeProducer);
                return _mapper.Map<IEnumerable<FarmSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de fincas del productor {producerId}.", ex);
            }
        }


        public async Task<FarmSelectDto> UpdateFarmAsync(FarmUpdateDto dto)
        {
            var entity = await _farmRepository.GetByIdAsync(dto.Id)
                ?? throw new Exception($"Product No se encontró el producto {dto.Id}");

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                dto.Adapt(entity);

                await _farmRepository.UpdateAsync(entity);

                if (dto.ImagesToDelete?.Any() == true)
                    await DeleteImagesAsync(dto.ImagesToDelete);

                if (dto.Images?.Any() == true)
                {
                    var validFiles = dto.Images.Where(f => f?.Length > 0).ToList();

                    var currentCount = (await _farmImageRepository.GetByFarmIdAsync(dto.Id)).Count;
                    ValidateMaxImages(validFiles.Count + currentCount, currentCount);

                    var newImages = await UploadAndMapImagesAsync(validFiles, entity.Id);
                    await _farmImageRepository.AddImages(newImages);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (await _farmRepository.GetByIdAsync(dto.Id))!.Adapt<FarmSelectDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando finca ID {Id}", dto.Id);
                await transaction.RollbackAsync();
                throw;
            }
        }

        #region Helpers
        private async Task<List<FarmImage>> UploadAndMapImagesAsync(IEnumerable<IFormFile>? files, int farmId)
        {
            if (files == null || !files.Any())
                return new List<FarmImage>();

            var semaphore = new SemaphoreSlim(3);
            var uploadTasks = files.Select(async file =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var uploadResult = await _cloudinaryService.UploadFarmImagesAsync(file, farmId);
                    return new FarmImage
                    {
                        FileName = file.FileName,
                        ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
                        PublicId = uploadResult.PublicId,
                        FarmId = farmId
                    };
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var images = (await Task.WhenAll(uploadTasks)).ToList();
            _logger.LogInformation("{Count} imágenes subidas para finca ID {Id}", images.Count, farmId);
            return images;
        }

        private void ValidateMaxImages(int totalImages, int currentCount = 0)
        {
            if (totalImages > MaxImages || totalImages > (MaxImages - currentCount))
                throw new BusinessException($"Solo se permiten hasta {MaxImages} imágenes por finca. Actualmente: {currentCount}.");
        }

        private async Task DeleteImagesAsync(IEnumerable<string> publicIds)
        {
            foreach (var publicId in publicIds.Where(id => !string.IsNullOrWhiteSpace(id)))
            {
                await _cloudinaryService.DeleteAsync(publicId);
                await _farmImageRepository.DeleteLogicalByPublicIdAsync(publicId);
            }
        }


        #endregion
    }
}
