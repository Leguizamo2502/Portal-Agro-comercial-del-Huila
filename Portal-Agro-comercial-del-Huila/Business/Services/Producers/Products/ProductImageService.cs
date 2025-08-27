using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Products;
using Business.Repository;
using Data.Interfaces.Implements.Producers.Products;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.DTOs.Products.Select;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using static System.Net.Mime.MediaTypeNames;

namespace Business.Services.Producers.Products
{
    public class ProductImageService : BusinessGeneric<ProductImageSelectDto,ProductImageSelectDto,ProductImage>,IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<ProductImageService> _logger;
        public ProductImageService(IDataGeneric<ProductImage> data, IMapper mapper, IProductImageRepository productImageRepository,
            ICloudinaryService cloudinaryService, ILogger<ProductImageService> logger) : base(data, mapper)
        {
            _productImageRepository = productImageRepository;
            _cloudinaryService = cloudinaryService;
            _logger  = logger;
        }


        public async Task<List<ProductImageSelectDto>> AddImagesAsync(int productId, IFormFileCollection files)
        {
            var filesToUpload = files.Take(5).ToList(); // no subir más de 5 nunca

            var imagesToAdd = new List<ProductImage>();

            try
            {
                // Subida paralela de imágenes a Cloudinary
                var uploadTasks = filesToUpload.Select(file => _cloudinaryService.UploadProductImagesAsync(file, productId));
                var uploadResults = await Task.WhenAll(uploadTasks);

                for (int i = 0; i < filesToUpload.Count; i++)
                {
                    var file = filesToUpload[i];
                    var uploadResult = uploadResults[i];

                    var image = new ProductImage
                    {
                        FileName = file.FileName,
                        ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
                        PublicId = uploadResult.PublicId,
                        ProductId = productId
                    };

                    imagesToAdd.Add(image);
                }

                // Llamada al repo para persistir con control transaccional
                await _productImageRepository.AddImages(imagesToAdd);

                return _mapper.Map<List<ProductImageSelectDto>>(imagesToAdd);
            }
            catch (InvalidOperationException ex) // capturamos la excepción de límite excedido en repo
            {
                // Opcional: eliminar imágenes subidas a Cloudinary para evitar recursos huérfanos
                var deleteTasks = imagesToAdd.Select(img => _cloudinaryService.DeleteAsync(img.PublicId));
                await Task.WhenAll(deleteTasks);

                throw new BusinessException(ex.Message);
            }
        }


        public async Task<List<ProductImageSelectDto>> GetImagesByProductIdAsync(int productId)
        {
            var images = await _productImageRepository.GetByProductIdAsync(productId);
            return _mapper.Map<List<ProductImageSelectDto>>(images);
        }

        public async Task DeleteImagesByPublicIdsAsync(List<string> publicIds)
        {
            if (publicIds == null || publicIds.Count == 0)
                return;

            foreach (var publicId in publicIds)
            {
                await _cloudinaryService.DeleteAsync(publicId);
                await _productImageRepository.DeleteByPublicIdAsync(publicId);
            }
        }

        /// <summary>
        /// Eliminar una imagen por ID
        /// </summary>
        /// 
        public async Task DeleteImageByIdAsync(int imageId)
        {
            var image = await _productImageRepository.GetByIdAsync(imageId)
                ?? throw new KeyNotFoundException("Imagen no encontrada");

            await _cloudinaryService.DeleteAsync(image.PublicId);
            await _productImageRepository.DeleteAsync(image.Id);
        }

        public async Task<bool> DeleteLogicalByPublicIdAsync(string publicId)
        {
            await _productImageRepository.DeleteLogicalByPublicIdAsync(publicId);

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

    }
}
