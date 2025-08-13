using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Products;
using Business.Repository;
using Data.Interfaces.Implements.Producers.Products;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Products;
using Entity.DTOs.Products;
using MapsterMapper;
using Utilities.Exceptions;

namespace Business.Services.Producers.Products
{
    public class ProductImageService : BusinessGeneric<ProductImageSelectDto,ProductImageSelectDto,ProductImage>,IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly ICloudinaryService _cloudinaryService;
        public ProductImageService(IDataGeneric<ProductImage> data, IMapper mapper, IProductImageRepository productImageRepository,
            ICloudinaryService cloudinaryService) : base(data, mapper)
        {
            _productImageRepository = productImageRepository;
            _cloudinaryService = cloudinaryService;
        }

        /// <summary>
        /// Eliminar una imagen por ID
        /// </summary>
        public async Task DeleteImageByIdAsync(int imageId)
        {
            var image = await _productImageRepository.GetByIdAsync(imageId)
                ?? throw new KeyNotFoundException("Imagen no encontrada");

            await _cloudinaryService.DeleteAsync(image.PublicId);
            await _productImageRepository.DeleteAsync(image.Id);
        }
    }
}
