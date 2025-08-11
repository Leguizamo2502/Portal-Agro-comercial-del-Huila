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
    public class ProductService : BusinessGeneric<ProductCreateDto, ProductSelectDto, Product>, IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly ICloudinaryService _cloudinaryService;
        public ProductService(IDataGeneric<Product> data, IMapper mapper, IProductRepository productRepository, ICloudinaryService cloudinaryService) : base(data, mapper)
        {
            _productRepository = productRepository;
            _cloudinaryService = cloudinaryService;
        }

        public override async Task<ProductCreateDto> CreateAsync(ProductCreateDto dto)
        {
            try
            {
                var product = _mapper.Map<Product>(dto);

                product = await _productRepository.AddAsync(product);

                var images = await _cloudinaryService.UploadProductImagesAsync(dto.Images, product.Id);
                product.ProductImages = images;

                await _productRepository.UpdateAsync(product);

                return _mapper.Map<ProductCreateDto>(product);

            }
            catch (Exception ex) {
                throw new BusinessException("No se puso crear el producto, verefica los datos",ex);
            }

        }

        public override async Task<IEnumerable<ProductSelectDto>> GetAllAsync()
        {
            try
            {
                var entities = await _productRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ProductSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de productos.", ex);
            }
        }

    }
}
