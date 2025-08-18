using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Products;
using Business.Repository;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.Implements.Producers.Products;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Products;
using Entity.DTOs.Products.Create;
using Entity.DTOs.Products.Select;
using Entity.DTOs.Products.Update;
using Entity.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers.Business;
using static System.Net.Mime.MediaTypeNames;

namespace Business.Services.Producers.Products
{
    public class ProductService : BusinessGeneric<ProductCreateDto, ProductSelectDto, Product>, IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ApplicationDbContext _context;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IProducerRepository _producerRepository;

        private const int MaxImages = 5;

        public ProductService(IDataGeneric<Product> data, IMapper mapper, IProductRepository productRepository, 
            ICloudinaryService cloudinaryService, IProductImageRepository productImageRepository,ApplicationDbContext context, 
            ILogger<ProductService> logger,IProducerRepository producerRepository) : base(data, mapper)
        {
            _productRepository = productRepository;
            _cloudinaryService = cloudinaryService;
            _productImageRepository = productImageRepository;
            _context = context;
            _logger = logger;
            _producerRepository = producerRepository;
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
        public override async Task<ProductSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var entity = await _productRepository.GetByIdAsync(id);
                return entity == null ? default : _mapper.Map<ProductSelectDto>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el rproducto con ID {id}.", ex);
            }
        }
        public override async Task<bool> DeleteAsync(int id)
        {
            var entity = await _productRepository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Intento de eliminar un producto inexistente con ID {Id}", id);
                return false;
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var images = await _productImageRepository.GetByProductIdAsync(id);

                foreach (var image in images)
                {
                    await _cloudinaryService.DeleteAsync(image.PublicId);
                    await _productImageRepository.DeleteLogicalByPublicIdAsync(image.PublicId);
                }
                await _context.SaveChangesAsync();

                var deleted = await _productRepository.DeleteLogicAsync(id);
                await _context.SaveChangesAsync();

                if (deleted)
                    _logger.LogInformation("Producto eliminado con ID {Id}", id);
                else
                    _logger.LogError("Error al eliminar eñ producto con ID {Id}", id);

                await transaction.CommitAsync();

                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando eñ producto ID {Id}", id);
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<ProductSelectDto> CreateProductAsync(ProductCreateDto dto)
        {
            ValidateMaxImages(dto.Images?.Count ?? 0);

            var entity = dto.Adapt<Product>();

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _productRepository.AddAsync(entity);
                await _context.SaveChangesAsync();

                var images = await UploadAndMapImagesAsync(dto.Images, entity.Id);
                if (images.Any())
                {
                    await _productImageRepository.AddImages(images);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                var result = entity.Adapt<ProductSelectDto>();
                result.Images = images.Adapt<List<ProductImageSelectDto>>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando producto");
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<ProductSelectDto> UpdateProductAsync(ProductUpdateDto dto)
        {
            var entity = await _productRepository.GetByIdAsync(dto.Id)
                ?? throw new Exception($"Product No se encontró el producto {dto.Id}");

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                dto.Adapt(entity);

                await _productRepository.UpdateAsync(entity);

                if (dto.ImagesToDelete?.Any() == true)
                    await DeleteImagesAsync(dto.ImagesToDelete);

                if (dto.Images?.Any() == true)
                {
                    var validFiles = dto.Images.Where(f => f?.Length > 0).ToList();

                    var currentCount = (await _productImageRepository.GetByProductIdAsync(dto.Id)).Count;
                    ValidateMaxImages(validFiles.Count + currentCount, currentCount);

                    var newImages = await UploadAndMapImagesAsync(validFiles, entity.Id);
                    await _productImageRepository.AddImages(newImages);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (await _productRepository.GetByIdAsync(dto.Id))!.Adapt<ProductSelectDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando producto ID {Id}", dto.Id);
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<IEnumerable<ProductSelectDto>> GetByProducer(int userId)
        {
            try
            {
                var producerId = await _producerRepository.GetIdProducer(userId);
                if (producerId == null)
                    throw new BusinessException("El usuario no está registrado como productor.");
                var entities = await _productRepository.GetByProducer(producerId);
                return _mapper.Map<IEnumerable<ProductSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de productos del productor {producerId}.", ex);
            }
        }



        #region Helpers
        private async Task<List<ProductImage>> UploadAndMapImagesAsync(IEnumerable<IFormFile>? files, int productId)
        {
            if (files == null || !files.Any())
                return new List<ProductImage>();

            var semaphore = new SemaphoreSlim(3);
            var uploadTasks = files.Select(async file =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var uploadResult = await _cloudinaryService.UploadProductImagesAsync(file, productId);
                    return new ProductImage
                    {
                        FileName = file.FileName,
                        ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
                        PublicId = uploadResult.PublicId,
                        ProductId = productId
                    };
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var images = (await Task.WhenAll(uploadTasks)).ToList();
            _logger.LogInformation("{Count} imágenes subidas para producto ID {Id}", images.Count, productId);
            return images;
        }

        private void ValidateMaxImages(int totalImages, int currentCount = 0)
        {
            if (totalImages > MaxImages || totalImages > (MaxImages - currentCount))
                throw new BusinessException($"Solo se permiten hasta {MaxImages} imágenes por producto. Actualmente: {currentCount}.");
        }

        private async Task DeleteImagesAsync(IEnumerable<string> publicIds)
        {
            foreach (var publicId in publicIds.Where(id => !string.IsNullOrWhiteSpace(id)))
            {
                await _cloudinaryService.DeleteAsync(publicId);
                await _productImageRepository.DeleteLogicalByPublicIdAsync(publicId);
            }
        }


        #endregion


    }
}
