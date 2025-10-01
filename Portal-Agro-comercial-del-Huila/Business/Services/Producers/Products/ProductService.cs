using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Products;
using Business.Repository;
using Data.Interfaces.Implements.Favorites;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.Implements.Producers.Products;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Favorites;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.DTOs.Products.Create;
using Entity.DTOs.Products.Select;
using Entity.DTOs.Products.Update;
using Entity.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Custom.Code;
using Utilities.Exceptions;
using Utilities.Helpers.Business;

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
        private readonly IFavoriteRepository _favoriteRepository;

        private const int MaxImages = 5;

        public ProductService(IDataGeneric<Product> data, IMapper mapper, IProductRepository productRepository, 
            ICloudinaryService cloudinaryService, IProductImageRepository productImageRepository,ApplicationDbContext context, 
            ILogger<ProductService> logger,IProducerRepository producerRepository,IFavoriteRepository favoriteRepository) : base(data, mapper)
        {
            _productRepository = productRepository;
            _cloudinaryService = cloudinaryService;
            _productImageRepository = productImageRepository;
            _context = context;
            _logger = logger;
            _producerRepository = producerRepository;
            _favoriteRepository = favoriteRepository;
        }


        public override async Task<bool> DeleteAsync(int id)
        {
            var entity = await _productRepository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Intento de eliminar un producto inexistente con ID {Id}", id);
                return false;
            }

            // 1) Obtener los publicIds de las imágenes (sin transacción)
            var imgs = await _productImageRepository.GetByProductIdAsync(id);
            var publicIds = imgs
                .Select(i => i.PublicId)
                .Where(pid => !string.IsNullOrWhiteSpace(pid))
                .Distinct()
                .ToList();

            var strategy = _context.Database.CreateExecutionStrategy();
            bool deleted = false;

            // 2) Unidad reintetable: solo lógica de BD + transacción manual dentro
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    foreach (var pid in publicIds)
                    {
                        await _productImageRepository.DeleteLogicalByPublicIdAsync(pid);
                    }

                    deleted = await _productRepository.DeleteLogicAsync(id);

                    await _context.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });

            // 3) IO externo fuera de la strategy/tx: eliminar en Cloudinary
            foreach (var pid in publicIds)
            {
                try
                {
                    await _cloudinaryService.DeleteAsync(pid);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Falló eliminación en Cloudinary para PublicId={PublicId} del producto {ProductId}", pid, id);
                }
            }

            if (deleted)
                _logger.LogInformation("Producto eliminado con ID {Id}", id);
            else
                _logger.LogError("Error al eliminar el producto con ID {Id}", id);

            return deleted;
        }



        public async Task<int> CreateProductAsync(ProductCreateDto dto)
        {
            ValidateMaxImages(dto.Images?.Count ?? 0);

            // Obtener Producer.Id real
            var pid = await _producerRepository.GetIdProducer(dto.ProducerId)
                     ?? throw new BusinessException("El usuario no está registrado como productor.");

            // Validaciones previas (sin transacción)
            var categoryExists = await _context.Category
                .AnyAsync(c => c.Id == dto.CategoryId && !c.IsDeleted);
            if (!categoryExists)
                throw new BusinessException("Categoría inválida.");

            var farmIds = (dto.FarmIds ?? new List<int>()).Distinct().ToList();
            if (farmIds.Count > 0)
            {
                var validCount = await _context.Farms
                    .CountAsync(f => farmIds.Contains(f.Id) && f.ProducerId == pid && !f.IsDeleted);
                if (validCount != farmIds.Count)
                    throw new BusinessException("Una o más fincas no pertenecen al productor.");
            }

            var strategy = _context.Database.CreateExecutionStrategy();
            int productId = 0;

            // Bloque reintetable con transacción manual dentro
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    var entity = dto.Adapt<Product>();
                    entity.ProducerId = pid;
                    entity.Active = true;
                    entity.IsDeleted = false;
                    entity.CreateAt = DateTime.UtcNow;

                    await _productRepository.AddAsync(entity);
                    await _context.SaveChangesAsync();
                    productId = entity.Id;

                    if (farmIds.Count > 0)
                    {
                        var now = DateTime.UtcNow;
                        var pivots = farmIds.Select(fid => new ProductFarm
                        {
                            ProductId = entity.Id,
                            FarmId = fid,
                            Active = true,
                            IsDeleted = false,
                            CreateAt = now
                        });
                        _context.ProductFarms.AddRange(pivots);
                        await _context.SaveChangesAsync();
                    }

                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });

            // IO externo fuera de la strategy para no duplicar subidas en reintentos
            List<ProductImage> images = new();
            try
            {
                images = await UploadAndMapImagesAsync(dto.Images, productId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Fallo al subir/mapear imágenes para producto {ProductId}", productId);
            }

            if (images.Any())
            {
                // Persistencia de imágenes (op: strategy sin tx manual)
                await _context.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
                {
                    await _productImageRepository.AddImages(images);
                    await _context.SaveChangesAsync();
                });
            }

            return productId;
        }


        public async Task<bool> UpdateProductAsync(ProductUpdateDto dto, int userId)
        {
            // 1) Validaciones previas (sin transacción)
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductFarms)
                .FirstOrDefaultAsync(p => p.Id == dto.Id && !p.IsDeleted)
                ?? throw new BusinessException($"Producto no encontrado: {dto.Id}");

            if (product.ProducerId != producerId)
                throw new BusinessException("No está autorizado para modificar este producto.");

            var categoryExists = await _context.Category
                .AnyAsync(c => c.Id == dto.CategoryId && !c.IsDeleted);
            if (!categoryExists)
                throw new BusinessException("Categoría inválida.");

            var newFarmIds = (dto.FarmIds ?? new List<int>()).Distinct().ToHashSet();
            if (newFarmIds.Count > 0)
            {
                var validCount = await _context.Farms
                    .CountAsync(f => newFarmIds.Contains(f.Id) && f.ProducerId == producerId && !f.IsDeleted);
                if (validCount != newFarmIds.Count)
                    throw new BusinessException("Una o más fincas no pertenecen al productor.");
            }

            // 2) Variables para IO externo (fuera de strategy/tx)
            //var imagesToDelete = dto.ImagesToDelete?.ToList() ?? new List<int>();
            var filesToUpload = (dto.Images ?? Enumerable.Empty<IFormFile>()).Where(f => f?.Length > 0).ToList();

            // 3) Bloque reintetable: solo lógica de BD + transacción manual dentro
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    // 3.1) Escalares
                    dto.Adapt(product);

                    // 3.2) Sincronizar N–M (ProductFarms) con soft delete
                    var currentActive = product.ProductFarms
                        .Where(pf => !pf.IsDeleted)
                        .Select(pf => pf.FarmId)
                        .ToHashSet();

                    var toAdd = newFarmIds.Except(currentActive).ToList();
                    var toRemove = currentActive.Except(newFarmIds).ToList();

                    var now = DateTime.UtcNow;

                    if (toAdd.Count > 0)
                    {
                        var pivots = toAdd.Select(fid => new ProductFarm
                        {
                            ProductId = product.Id,
                            FarmId = fid,
                            Active = true,
                            IsDeleted = false,
                            CreateAt = now
                        });
                        _context.ProductFarms.AddRange(pivots);
                    }

                    if (toRemove.Count > 0)
                    {
                        foreach (var fid in toRemove)
                        {
                            var pivot = product.ProductFarms.First(pf => pf.FarmId == fid && !pf.IsDeleted);
                            pivot.IsDeleted = true;
                            pivot.Active = false;
                        }
                    }

                    // OJO: aquí NO hacemos IO externo (subida/borrado en nube)

                    await _context.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });

            // 4) IO externo fuera (idempotente)
            List<ProductImage> newImages = new();
            try
            {
                //if (imagesToDelete.Count > 0)
                //{
                //    // Si DeleteImagesAsync toca nube, que lo haga aquí (fuera del strategy/tx)
                //    await DeleteImagesAsync(imagesToDelete);
                //}

                if (filesToUpload.Count > 0)
                {
                    // Validar cupo antes de subir
                    var currentCount = (await _productImageRepository.GetByProductIdAsync(dto.Id)).Count;
                    //ValidateMaxImages(filesToUpload.Count + currentCount, currentCount);

                    // Subida a nube + creación de entidades en memoria
                    newImages = await UploadAndMapImagesAsync(filesToUpload, product.Id);
                }
            }
            catch (Exception ex)
            {
                // No rompas la actualización del producto por fallo de IO externo
                _logger.LogWarning(ex, "Fallo IO externo (imágenes) para producto {ProductId}", product.Id);
            }

            // 5) Persistencia de imágenes (solo BD) bajo strategy, sin tx manual
            if (newImages.Any())
            {
                await _context.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
                {
                    await _productImageRepository.AddImages(newImages);
                    await _context.SaveChangesAsync();
                });
            }

            return true;
        }


        public async Task<bool> AddFavoriteAsync(int userId, int productId)
        {
            if (await _favoriteRepository.ExistsAsync(userId, productId))
                return false;

            try
            {
                var entity = new Favorite { UserId = userId, ProductId = productId };
                await _favoriteRepository.AddAsync(entity);
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveFavoriteAsync(int userId, int productId)
        {
            try
            {
                var entity = await _favoriteRepository.GetByFavoriteAsync(userId, productId);

                if (entity is null)
                    return false;

                return await _favoriteRepository.DeleteAsync(entity.Id);
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateStockAsync(UpdateStockDto dto)
        {
            try
            {
                var updated = await _productRepository.UpdateStock(dto.ProductId, dto.NewStock);

                if (!updated)
                    throw new BusinessException($"No se encontró el producto {dto.ProductId} o no se pudo actualizar el stock.");

                return true;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error actualizando stock. ProductId={ProductId}, NewStock={NewStock}",
                    dto.ProductId, dto.NewStock);

                throw new BusinessException("No se pudo actualizar el stock por un error interno.");
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
