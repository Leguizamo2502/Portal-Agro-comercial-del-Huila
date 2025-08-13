using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Products;
using Business.Repository;
using Data.Interfaces.Implements.Producers.Products;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Products;
using Entity.DTOs.Products;
using Entity.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
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

        private const int MaxImages = 5;

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
                _logger.LogError(ex, "Error creando establecimiento");
                await transaction.RollbackAsync();
                throw;
            }
        }




        public async Task<ProductSelectDto> UpdateProductAsync(ProductUpdateDto dto)
        {
            var entity = await _productRepository.GetByIdAsync(dto.Id)
                ?? throw new Exception($"Product No se encontró el establecimiento {dto.Id}");

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
                _logger.LogError(ex, "Error actualizando establecimiento ID {Id}", dto.Id);
                await transaction.RollbackAsync();
                throw;
            }
        }


        //public async Task<ProductSelectDto> UpdateProductAsync(ProductUpdateDto dto)
        //{
        //    // 0) Cargar entidad
        //    var entity = await _productRepository.GetByIdAsync(dto.Id);
        //    if (entity == null)
        //        throw new BusinessException($"No se encontró el producto {dto.Id}");

        //    // 1) Leer imágenes actuales (solo activas)
        //    var currentImages = await _productImageRepository.GetByProductIdAsync(dto.Id);
        //    // Si tu repo trae eliminadas lógicamente, descomenta la siguiente línea:
        //    // currentImages = currentImages.Where(i => !i.IsDeleted).ToList();

        //    // 2) Normalizar entradas del DTO
        //    const int MAX_IMAGES = 5;

        //    var toDelete = (dto.ImagesToDelete ?? new List<string>())
        //        .Where(s => !string.IsNullOrWhiteSpace(s))
        //        .Select(s => s.Trim())
        //        .ToHashSet(StringComparer.OrdinalIgnoreCase);

        //    var validFiles = (dto.Images ?? new List<IFormFile>())
        //        .Where(f => f is { Length: > 0 })
        //        .ToList();

        //    // 3) Calcular capacidad efectiva (considerando borrados de esta solicitud)
        //    var currentPublicIds = new HashSet<string>(
        //        currentImages
        //            .Where(i => !string.IsNullOrWhiteSpace(i.PublicId))
        //            .Select(i => i.PublicId.Trim()),
        //        StringComparer.OrdinalIgnoreCase
        //    );

        //    var deletionsAffectingCount = currentPublicIds.Intersect(toDelete).Count();
        //    var plannedAfter = currentImages.Count - deletionsAffectingCount;
        //    if (plannedAfter < 0) plannedAfter = 0;

        //    var spaceAvailable = Math.Max(0, MAX_IMAGES - plannedAfter);

        //    _logger.LogInformation(
        //        "UpdateProduct Images: current={Current}, toDelete={ToDelete}, matchDeletes={Match}, plannedAfter={After}, spaceAvailable={Space}, toUpload={Upload}",
        //        currentImages.Count, toDelete.Count, deletionsAffectingCount, plannedAfter, spaceAvailable, validFiles.Count
        //    );

        //    if (validFiles.Count > spaceAvailable)
        //        throw new BusinessException(
        //            $"Solo puede subir {spaceAvailable} imagen(es) adicional(es). Máximo {MAX_IMAGES} por producto."
        //        );

        //    // 4) Cloudinary FUERA de la transacción
        //    // 4.1) Borrar en Cloudinary
        //    if (toDelete.Count > 0)
        //    {
        //        foreach (var publicId in toDelete)
        //            await _cloudinaryService.DeleteAsync(publicId);
        //    }

        //    // 4.2) Subir nuevas
        //    var newImages = new List<ProductImage>();
        //    if (validFiles.Count > 0)
        //        newImages = await UploadAndMapImagesAsync(validFiles, entity.Id);

        //    // 5) Persistencia BD en transacción CORTA
        //    await using var tx = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        // 5.1) Actualizar campos escalares (DTO con nullables)
        //        entity.Name = dto.Name ?? entity.Name;
        //        entity.Description = dto.Description ?? entity.Description;
        //        entity.Price = dto.Price ?? entity.Price;
        //        entity.Stock = dto.Stock ?? entity.Stock;
        //        entity.Production = dto.Production ?? entity.Production;
        //        entity.CategoryId = dto.CategoryId ?? entity.CategoryId;
        //        entity.Status = dto.Status ?? entity.Status;
        //        entity.FarmId = dto.FarmId ?? entity.FarmId;

        //        await _productRepository.UpdateAsync(entity); // sin SaveChanges/tx internas

        //        // 5.2) Marcar borrados en BD por PublicId
        //        if (toDelete.Count > 0)
        //        {
        //            foreach (var publicId in toDelete)
        //                await _productImageRepository.DeleteLogicalByPublicIdAsync(publicId);
        //        }

        //        // 5.3) Insertar nuevas imágenes en BD
        //        if (newImages.Count > 0)
        //            await _productImageRepository.AddImages(newImages);

        //        await _context.SaveChangesAsync();
        //        await tx.CommitAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error actualizando producto ID {Id}", dto.Id);
        //        // Sin RollbackAsync: el Dispose del await using hará rollback automático
        //        throw;
        //    }

        //    // 6) Leer actualizado y devolver
        //    var updated = await _productRepository.GetByIdAsync(dto.Id);

        //    // Si tu configuración Mapster ya mapea Images, usa:
        //    return _mapper.Map<ProductSelectDto>(updated!);

        //    // Si prefieres asignar Images manualmente, usa este bloque en su lugar:
        //    /*
        //    var resultDto = _mapper.Map<ProductSelectDto>(updated!);
        //    if (updated?.ProductImages != null)
        //    {
        //        resultDto.Images = updated.ProductImages
        //            .Select(i => new ProductImageSelectDto(i.Id, i.FileName, i.ImageUrl, i.PublicId, i.ProductId))
        //            .ToList();
        //    }
        //    return resultDto;
        //    */
        //}





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
            _logger.LogInformation("{Count} imágenes subidas para establecimiento ID {Id}", images.Count, productId);
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



        #region Helpers

        private void ValidateMaxImages(int totalImages, int currentCount = 0)
        {
            if (totalImages > MaxImages || totalImages > (MaxImages - currentCount))
                throw new BusinessException($"Solo se permiten hasta {MaxImages} imágenes por establecimiento. Actualmente: {currentCount}.");
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
