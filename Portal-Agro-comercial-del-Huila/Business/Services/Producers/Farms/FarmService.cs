
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
using Microsoft.EntityFrameworkCore;
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
                _logger.LogWarning("Intento de eliminar una finca inexistente con ID {Id}", id);
                return false;
            }

            // 1) Obtener publicIds (sin transacción)
            var imgs = await _farmImageRepository.GetByFarmIdAsync(id);
            var publicIds = imgs
                .Select(i => i.PublicId)
                .Where(pid => !string.IsNullOrWhiteSpace(pid))
                .Distinct()
                .ToList();

            var strategy = _context.Database.CreateExecutionStrategy();
            bool deleted = false;

            // 2) Unidad reintetable: solo BD + transacción manual dentro
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Borrado lógico de imágenes en BD
                    foreach (var pid in publicIds)
                    {
                        await _farmImageRepository.DeleteLogicalByPublicIdAsync(pid);
                    }

                    // Borrado lógico de la finca
                    deleted = await _farmRepository.DeleteLogicAsync(id);

                    await _context.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });

            // 3) IO externo fuera de strategy/tx: eliminar en Cloudinary
            foreach (var pid in publicIds)
            {
                try
                {
                    await _cloudinaryService.DeleteAsync(pid);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "Falló eliminación en Cloudinary para PublicId={PublicId} de la finca {FarmId}", pid, id);
                }
            }

            if (deleted)
                _logger.LogInformation("Finca eliminada con ID {Id}", id);
            else
                _logger.LogError("Error al eliminar la finca con ID {Id}", id);

            return deleted;
        }


        public async Task<FarmSelectDto> RegisterWithProducer(ProducerWithFarmRegisterDto dto, int userId)
        {
            // 0) Validaciones iniciales
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new BusinessException("Usuario no encontrado");

            if (user.Producer != null)
                throw new BusinessException("El usuario ya es productor");

            ValidateMaxImages(dto.Images?.Count ?? 0);

            // Variables que necesitaremos fuera del bloque reintetable
            Producer? producer = null;
            Farm? farm = null;
            List<FarmImage>? images = null;

            var strategy = _context.Database.CreateExecutionStrategy();

            // 1) Ejecutar todo como unidad reintetable (transacción incluida)
            var result = await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // 1. Crear Productor
                    producer = dto.Adapt<Producer>();
                    producer!.Id = 0;
                    producer.UserId = user.Id;
                    producer.User = null;
                    producer.Code = CodeGenerator.Generate(10);

                    await _producerRepository.AddAsync(producer);
                    await _context.SaveChangesAsync();

                    // 1.1 Redes sociales
                    if (dto.SocialLinks != null && dto.SocialLinks.Count > 0)
                    {
                        var duplicated = dto.SocialLinks
                            .GroupBy(x => x.Network)
                            .FirstOrDefault(g => g.Count() > 1);
                        if (duplicated != null)
                            throw new BusinessException($"Red social duplicada: {duplicated.Key}");

                        var links = dto.SocialLinks
                            .Select(sl =>
                            {
                                var url = Urls.NormalizeUrl(sl.Network, sl.Url);
                                return new ProducerSocialLink
                                {
                                    ProducerId = producer!.Id,
                                    Network = sl.Network,
                                    Url = url
                                };
                            })
                            .ToList();

                        await _context.Set<ProducerSocialLink>().AddRangeAsync(links);
                        await _context.SaveChangesAsync();
                    }

                    // 2) Rol de productor
                    await _rolUserRepository.AsignateRolProducer(user);
                    await _context.SaveChangesAsync();

                    // 3) Crear Finca (sin imágenes todavía)
                    farm = dto.Adapt<Farm>();
                    farm!.Id = 0;
                    farm.ProducerId = producer!.Id;

                    await _farmRepository.AddAsync(farm);
                    await _context.SaveChangesAsync();

                    // 4) Subir y mapear imágenes
                    images = await UploadAndMapImagesAsync(dto.Images, farm.Id);
                    if (images.Any())
                    {
                        await _farmImageRepository.AddImages(images);
                        await _context.SaveChangesAsync();
                    }

                    // 5) Commit
                    await transaction.CommitAsync();

                    // 6) DTO de salida coherente
                    var dtoOut = farm.Adapt<FarmSelectDto>();
                    dtoOut.Images = (images ?? new List<FarmImage>()).Adapt<List<FarmImageSelectDto>>();
                    return dtoOut;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });

            // 7) Generación/subida de QR fuera de la strategy (para no reintentar IO externo)
            try
            {
                if (producer != null)
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
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falló generación/subida de QR para productor {ProducerId}", producer?.Id);
                // No romper el flujo principal por fallo del QR
            }

            return result;
        }


        public async Task<FarmRegisterDto> CreateFarmAsync(FarmRegisterDto dto)
        {
            ValidateMaxImages(dto.Images?.Count ?? 0);

            var pid = await _producerRepository.GetIdProducer(dto.ProducerId)
                     ?? throw new BusinessException("El usuario no está registrado como productor.");

            var entity = dto.Adapt<Farm>();
            entity.ProducerId = pid;

            var strategy = _context.Database.CreateExecutionStrategy();

            // 1) Crear la finca dentro de la strategy + transacción manual
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _farmRepository.AddAsync(entity);
                    await _context.SaveChangesAsync(); // entity.Id ya disponible

                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });

            // 2) IO externo: subir imágenes fuera del bloque reintetable
            List<FarmImage> images = new();
            try
            {
                images = await UploadAndMapImagesAsync(dto.Images, entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Fallo al subir/mapear imágenes para la finca {FarmId}", entity.Id);
            }

            // 3) Persistir metadatos de imágenes (solo BD) bajo strategy, sin transacción manual
            if (images.Any())
            {
                await _context.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
                {
                    await _farmImageRepository.AddImages(images);
                    await _context.SaveChangesAsync();
                });
            }

            // 4) Resultado
            var result = entity.Adapt<FarmRegisterDto>();
            return result;
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
                ?? throw new BusinessException($"Finca no encontrada: {dto.Id}");

            // Preparar datos de IO externo
            var imagesToDelete = dto.ImagesToDelete?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
            var filesToUpload = (dto.Images ?? Enumerable.Empty<IFormFile>()).Where(f => f?.Length > 0).ToList();

            var strategy = _context.Database.CreateExecutionStrategy();

            // ===== 1) Solo BD (strategy + tx manual DENTRO). Sin IO externo. =====
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    // 1.1 Escalares
                    dto.Adapt(entity);
                    await _farmRepository.UpdateAsync(entity);

                    // 1.2 Borrado lógico de imágenes (NO Cloudinary aquí)
                    if (imagesToDelete.Count > 0)
                    {
                        foreach (var pid in imagesToDelete)
                            await _farmImageRepository.DeleteLogicalByPublicIdAsync(pid);
                    }

                    // 1.3 Validar cupo para nuevas imágenes con el conteo ACTUAL tras los borrados lógicos
                    var currentCount = (await _farmImageRepository.GetByFarmIdAsync(dto.Id)).Count;
                    //ValidateMaxImages(filesToUpload.Count + currentCount, currentCount);

                    await _context.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });

            // ===== 2) IO externo FUERA (subida a Cloudinary) =====
            List<FarmImage> newImages = new();
            if (filesToUpload.Count > 0)
            {
                try
                {
                    newImages = await UploadAndMapImagesAsync(filesToUpload, entity.Id); // sube a Cloudinary y crea entidades en memoria
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Fallo de subida de imágenes para finca {FarmId}", entity.Id);
                }
            }

            // ===== 3) Persistir metadatos de nuevas imágenes (solo BD), sin tx manual =====
            if (newImages.Any())
            {
                await _context.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
                {
                    await _farmImageRepository.AddImages(newImages);
                    await _context.SaveChangesAsync();
                });
            }

            // ===== 4) Borrar en Cloudinary (no romper si falla) =====
            if (imagesToDelete.Count > 0)
            {
                foreach (var pid in imagesToDelete)
                {
                    try { await _cloudinaryService.DeleteAsync(pid); }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Falló eliminación en Cloudinary para PublicId={PublicId} (FarmId={FarmId})", pid, entity.Id);
                    }
                }
            }

            // ===== 5) Devolver estado actualizado =====
            var updated = await _farmRepository.GetByIdAsync(dto.Id);
            return updated!.Adapt<FarmSelectDto>();
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
