using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Products;
using Business.Repository;
using Data.Interfaces.Implements.Favorites;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.Implements.Producers.Products;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Favorites;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.DTOs.Favorites.Create;
using Entity.DTOs.Products.Create;
using Entity.DTOs.Products.Select;
using Entity.DTOs.Products.Update;
using Entity.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers.Business;
using static System.Net.Mime.MediaTypeNames;
using static Dapper.SqlMapper;

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

        public async Task<IEnumerable<ProductSelectDto>> GetAllForUsersAsync(int userId)
        {
            try
            {
                var entities = await _productRepository.GetAllAsync();

                // Llamas al repo de favoritos (sin LINQ aquí)
                var favoriteIds = await _favoriteRepository.GetFavoriteProductIdsByUserAsync(userId);

                var dtos = _mapper.Map<List<ProductSelectDto>>(entities); // <= LIST en vez de IEnumerable

                foreach (var dto in dtos)
                    dto.IsFavorite = favoriteIds.Contains(dto.Id); // dto.Id = ProductId

                return dtos;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de productos.", ex);
            }
        }

        public async Task<IEnumerable<ProductSelectDto>> GetFavoritesForUsersAsync(int userId)
        {
            try
            {
                var favoriteIds = await _favoriteRepository.GetFavoriteProductIdsByUserAsync(userId);

                if (favoriteIds == null || !favoriteIds.Any())
                    return Enumerable.Empty<ProductSelectDto>();

                var favoriteProducts = await _productRepository.GetByIdsFavoritesAsync(favoriteIds);

                var dtos = _mapper.Map<List<ProductSelectDto>>(favoriteProducts);


                foreach (var dto in dtos)
                    dto.IsFavorite = true;

                return dtos;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener productos favoritos del usuario.", ex);
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


        public async Task<int> CreateProductAsync(ProductCreateDto dto)
        {
            ValidateMaxImages(dto.Images?.Count ?? 0);

            // dto.ProducerId viene con el userId; obtenemos el Producer.Id real
            var pid = await _producerRepository.GetIdProducer(dto.ProducerId)
                     ?? throw new BusinessException("El usuario no está registrado como productor.");

            // Validaciones
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

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = dto.Adapt<Product>();
                entity.ProducerId = pid;
                entity.Active = true;
                entity.IsDeleted = false;
                entity.CreateAt = DateTime.UtcNow;

                await _productRepository.AddAsync(entity);
                await _context.SaveChangesAsync(); // ya tienes entity.Id

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

                var images = await UploadAndMapImagesAsync(dto.Images, entity.Id);
                if (images.Any())
                {
                    await _productImageRepository.AddImages(images);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return entity.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDto dto, int userId)
        {
            // Producer del usuario
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            // Cargar producto con tracking
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductFarms)
                .FirstOrDefaultAsync(p => p.Id == dto.Id && !p.IsDeleted)
                ?? throw new BusinessException($"Producto no encontrado: {dto.Id}");

            if (product.ProducerId != producerId)
                throw new BusinessException("No está autorizado para modificar este producto.");

            // Validaciones
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

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                // Actualizar escalares (Mapster configurado para no tocar navs)
                dto.Adapt(product);

                // Sincronizar N–M (soft delete por índice único filtrado)
                var currentActive = product.ProductFarms
                    .Where(pf => !pf.IsDeleted)
                    .Select(pf => pf.FarmId)
                    .ToHashSet();

                var toAdd = newFarmIds.Except(currentActive).ToList();
                var toRemove = currentActive.Except(newFarmIds).ToList();

                var now = DateTime.UtcNow;

                foreach (var fid in toAdd)
                {
                    _context.ProductFarms.Add(new ProductFarm
                    {
                        ProductId = product.Id,
                        FarmId = fid,
                        Active = true,
                        IsDeleted = false,
                        CreateAt = now
                    });
                }

                foreach (var fid in toRemove)
                {
                    var pivot = product.ProductFarms.First(pf => pf.FarmId == fid && !pf.IsDeleted);
                    pivot.IsDeleted = true;
                    pivot.Active = false;
                }

                // Imágenes
                if (dto.ImagesToDelete?.Any() == true)
                    await DeleteImagesAsync(dto.ImagesToDelete);

                if (dto.Images?.Any() == true)
                {
                    var validFiles = dto.Images.Where(f => f?.Length > 0).ToList();
                    var currentCount = (await _productImageRepository.GetByProductIdAsync(dto.Id)).Count;

                    ValidateMaxImages(validFiles.Count + currentCount, currentCount);

                    var newImages = await UploadAndMapImagesAsync(validFiles, product.Id);
                    await _productImageRepository.AddImages(newImages);
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
                return true;
            }
            catch
            {
                await tx.RollbackAsync();
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
