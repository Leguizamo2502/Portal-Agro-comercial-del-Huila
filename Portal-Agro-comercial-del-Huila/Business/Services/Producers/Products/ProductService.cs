using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Products;
using Business.Repository;
using Data.Interfaces.Implements.Producers.Products;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Products;
using Entity.DTOs.Products;
using Entity.Infrastructure.Context;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services.Producers.Products
{
    public class ProductService : BusinessGeneric<ProductCreateDto, ProductSelectDto, Product>, IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ApplicationDbContext _context;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IDataGeneric<Product> data, IMapper mapper, IProductRepository productRepository, 
            ICloudinaryService cloudinaryService, IProductImageRepository productImageRepository,ApplicationDbContext context, 
            ILogger<ProductService> logger) : base(data, mapper)
        {
            _productRepository = productRepository;
            _cloudinaryService = cloudinaryService;
            _productImageRepository = productImageRepository;
            _context = context;
            _logger = logger;
        }

        public override async Task<ProductCreateDto> CreateAsync(ProductCreateDto dto)
        {
            if (dto.Images == null || dto.Images.Count == 0)
                throw new BusinessException("Debe subir al menos una imagen.");

            if (dto.Images.Count > 5)
                throw new BusinessException("Solo se permiten hasta 5 imágenes por producto.");
            var product = _mapper.Map<Product>(dto);
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                product = await _productRepository.AddAsync(product);
                await _context.SaveChangesAsync();

                var images = await UploadAndMapImagesAsync(dto.Images, product.Id);
                if (images.Any())
                {
                    await _productImageRepository.AddImages(images);
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();

                //product.ProductImages = images;

                await _productRepository.UpdateAsync(product);

                return _mapper.Map<ProductCreateDto>(product);

            }
            catch (Exception ex) {
                throw new BusinessException("No se puso crear el producto, verefica los datos",ex);
            }

            //await using var transaction = await _context.Database.BeginTransactionAsync();
            //try
            //{
            //    await _repo.AddAsync(entity);
            //    await _context.SaveChangesAsync(); // Guarda para obtener Id

            //    var images = await UploadAndMapImagesAsync(dto.Files, entity.Id);

            //    if (images.Any())
            //    {
            //        await _imagesRepo.AddAsync(images);
            //        await _context.SaveChangesAsync(); // Guarda imágenes
            //    }

            //    await transaction.CommitAsync();

            //    var result = _mapper.Map<EstablishmentSelectDto>(entity);
            //    result.Images = images.Adapt<List<ImageSelectDto>>();
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error creando establecimiento");
            //    await transaction.RollbackAsync();
            //    throw;
            //}

        }


        private async Task<List<ProductImage>> UploadAndMapImagesAsync(List<IFormFile>? files, int productId)
        {
            if (files == null || !files.Any())
                return new List<ProductImage>();

            var fileList = files.ToList();

            // Limitar concurrencia para no saturar Cloudinary
            var semaphore = new SemaphoreSlim(3);

            var uploadTasks = fileList.Select(async file =>
            {
                await semaphore.WaitAsync();
                try
                {
                    return await _cloudinaryService.UploadProductImagesAsync(file, productId);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToList();

            var uploadResults = await Task.WhenAll(uploadTasks);

            var images = new List<ProductImage>();

            for (int i = 0; i < fileList.Count; i++)
            {
                images.Add(new ProductImage
                {
                    FileName = fileList[i].FileName,
                    ImageUrl = uploadResults[i].SecureUrl.AbsoluteUri,
                    PublicId = uploadResults[i].PublicId,
                    ProductId = productId
                });
            }

            _logger.LogInformation("{Count} imágenes subidas para producto ID {Id}",
                                   images.Count, productId);

            return images;
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

        public async Task<IEnumerable<ProductSelectDto>> GetByProducer(int producerId)
        {
            try
            {
                var entities = await _productRepository.GetByProducer(producerId);
                return _mapper.Map<IEnumerable<ProductSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de productos del productor {producerId}.", ex);
            }
        }
    }
}
