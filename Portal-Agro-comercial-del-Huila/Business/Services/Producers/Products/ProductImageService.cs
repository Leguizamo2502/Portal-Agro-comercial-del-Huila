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
    public class ProductImageService : BusinessGeneric<ProductImageDto,ProductImageDto,ProductImage>,IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly ICloudinaryService _cloudinaryService;
        public ProductImageService(IDataGeneric<ProductImage> data, IMapper mapper, IProductImageRepository productImageRepository,
            ICloudinaryService cloudinaryService) : base(data, mapper)
        {
            _productImageRepository = productImageRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var image = await _productImageRepository.GetByIdAsync(imageId);
            if (image == null)
                throw new BusinessException("La imagen no existe.");

            var publicId = _cloudinaryService.ExtractPublicId(image.ImageUrl);
            await _cloudinaryService.DeleteImageAsync(publicId);

            await _productImageRepository.DeleteAsync(image.Id);
        }
    }
}
