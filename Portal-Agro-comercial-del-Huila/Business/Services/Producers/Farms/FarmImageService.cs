using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Farms;
using Business.Repository;
using Data.Interfaces.Implements.Producers.Farms;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers.Farms;
using Entity.DTOs.Producer.Farm.Select;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services.Producers.Farms
{
    public class FarmImageService : BusinessGeneric<FarmImageSelectDto, FarmImageSelectDto, FarmImage>, IFarmImageService
    {
        private readonly IFarmImageRepository _farmImageRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<FarmImageService> _logger;
        public FarmImageService(IDataGeneric<FarmImage> data, IMapper mapper, IFarmImageRepository farmImageRepository, 
            ICloudinaryService cloudinary,ILogger<FarmImageService> logger) : base(data, mapper)
        {
            _farmImageRepository = farmImageRepository;
            _cloudinaryService = cloudinary;
            _logger = logger;
        }

        public async Task<List<FarmImageSelectDto>> AddImagesAsync(int farmId, IFormFileCollection files)
        {
            var filesToUpload = files.Take(5).ToList(); // no subir más de 5 nunca

            var imagesToAdd = new List<FarmImage>();

            try
            {
                // Subida paralela de imágenes a Cloudinary
                var uploadTasks = filesToUpload.Select(file => _cloudinaryService.UploadProductImagesAsync(file, farmId));
                var uploadResults = await Task.WhenAll(uploadTasks);

                for (int i = 0; i < filesToUpload.Count; i++)
                {
                    var file = filesToUpload[i];
                    var uploadResult = uploadResults[i];

                    var image = new FarmImage
                    {
                        FileName = file.FileName,
                        ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
                        PublicId = uploadResult.PublicId,
                        FarmId = farmId
                    };

                    imagesToAdd.Add(image);
                }

                // Llamada al repo para persistir con control transaccional
                await _farmImageRepository.AddImages(imagesToAdd);

                return _mapper.Map<List<FarmImageSelectDto>>(imagesToAdd);
            }
            catch (InvalidOperationException ex) // capturamos la excepción de límite excedido en repo
            {
                // Opcional: eliminar imágenes subidas a Cloudinary para evitar recursos huérfanos
                var deleteTasks = imagesToAdd.Select(img => _cloudinaryService.DeleteAsync(img.PublicId));
                await Task.WhenAll(deleteTasks);

                throw new BusinessException(ex.Message);
            }
        }

        public async Task DeleteImageByIdAsync(int imageId)
        {
            var image = await _farmImageRepository.GetByIdAsync(imageId)
                ?? throw new KeyNotFoundException("Imagen no encontrada");

            await _cloudinaryService.DeleteAsync(image.PublicId);
            await _farmImageRepository.DeleteAsync(image.Id);
        }

        public async Task DeleteImagesByPublicIdsAsync(List<string> publicIds)
        {
            if (publicIds == null || publicIds.Count == 0)
                return;

            foreach (var publicId in publicIds)
            {
                await _cloudinaryService.DeleteAsync(publicId);
                await _farmImageRepository.DeleteByPublicIdAsync(publicId);
            }
        }

        public async Task<bool> DeleteLogicalByPublicIdAsync(string publicId)
        {
        await _farmImageRepository.DeleteLogicalByPublicIdAsync(publicId);

            _ = Task.Run(async () =>
            {
                try
                {
                    await _cloudinaryService.DeleteAsync(publicId);
                }
                catch (Exception ex)
                {
                    // Loggear error para diagnóstico
                    _logger.LogError(ex, $"Error al eliminar de Cloudinary: {publicId}");
                }
            });

            return true;
        }

        public async Task<List<FarmImageSelectDto>> GetImagesByFarmIdAsync(int farmId)
        {
            var images = await _farmImageRepository.GetByFarmIdAsync(farmId);
            return _mapper.Map<List<FarmImageSelectDto>>(images);
        }





    }
}
