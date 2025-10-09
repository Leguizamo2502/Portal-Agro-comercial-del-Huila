// Business/Services/Producers/Products/ProductReadService.cs
using Business.Interfaces.Implements.Producers.Products;
using Data.Interfaces.Implements.Favorites;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.Implements.Producers.Products;
using Entity.DTOs.Products.Select;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

public class ProductReadService : IProductReadService
{
    private readonly IProductRepository _productRepo;
    private readonly IFavoriteRepository _favoriteRepo;
    private readonly IProducerRepository _producerRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductReadService> _logger;

    public ProductReadService(
        IProductRepository productRepo,
        IFavoriteRepository favoriteRepo,
        IProducerRepository producerRepo,
        IMapper mapper,
        ILogger<ProductReadService> logger)
    {
        _productRepo = productRepo;
        _favoriteRepo = favoriteRepo;
        _producerRepo = producerRepo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductSelectDto>> GetAllAsync()
    {
        try
        {
            var entities = await _productRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductSelectDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los productos.");
            throw new BusinessException("Error al obtener todos los productos.", ex);
        }
    }

    public async Task<ProductSelectDto?> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0) throw new BusinessException("El ID debe ser mayor que cero.");
            var entity = await _productRepo.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<ProductSelectDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el producto con ID {Id}", id);
            throw new BusinessException($"Error al obtener el producto con ID {id}.", ex);
        }
    }

    public async Task<ProductSelectDto?> GetDetailProduct(int? userId,int productId)
    {
        try
        {
            if (productId <= 0)
                throw new BusinessException("El ID debe ser mayor que cero.");

            var entity = await _productRepo.GetByIdAsync(productId);
            if (entity is null) return null;

            var dto = _mapper.Map<ProductSelectDto>(entity);

            if (userId is null)
            {
                dto.IsFavorite = false;
                return dto;
            }

            var favIds = await _favoriteRepo.GetFavoriteProductIdsByUserAsync(userId.Value);
            dto.IsFavorite = favIds.Contains(productId);

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el producto con ID {Id}", productId);
            throw new BusinessException($"Error al obtener el producto con ID {productId}.", ex);
        }
    }
   

    public async Task<IEnumerable<ProductSelectDto>> GetAllHomeAsync(int? limit)
    {
        try
        {
            var products = await _productRepo.GetAllWithLimitAsync(limit);
            return _mapper.Map<List<ProductSelectDto>>(products);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener Home");
            throw;
        }
    }

    public async Task<IEnumerable<ProductSelectDto>> GetFavoritesForUserAsync(int userId)
    {
        try
        {
            var favoriteIds = await _favoriteRepo.GetFavoriteProductIdsByUserAsync(userId);
            if (favoriteIds is null || !favoriteIds.Any()) return Enumerable.Empty<ProductSelectDto>();

            var products = await _productRepo.GetByIdsFavoritesAsync(favoriteIds);
            var dtos = _mapper.Map<List<ProductSelectDto>>(products);
            foreach (var d in dtos) d.IsFavorite = true;
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener favoritos del usuario {UserId}", userId);
            throw new BusinessException("Error al obtener productos favoritos del usuario.", ex);
        }
    }

    public async Task<IEnumerable<ProductSelectDto>> GetByProducerAsync(int userId)
    {
        try
        {
            var producerId = await _producerRepo.GetIdProducer(userId)
                ?? throw new BusinessException("El usuario no está registrado como productor.");

            var entities = await _productRepo.GetByProducer(producerId);
            return _mapper.Map<IEnumerable<ProductSelectDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos del productor para el usuario {UserId}", userId);
            throw new BusinessException("Error al obtener los productos del productor.", ex);
        }
    }

    public async Task<IEnumerable<ProductSelectDto>> GetByProducerCodeAsync(string codeProducer)
    {
        try
        {
            

            var entities = await _productRepo.GetByProducerCode(codeProducer);
            return _mapper.Map<IEnumerable<ProductSelectDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos del productor para el usuario {UserId}", codeProducer);
            throw new BusinessException("Error al obtener los productos del productor.", ex);
        }
    }


    public async Task<IEnumerable<ProductSelectDto>> GetByCategoryAsync(int categoryId)
    {
        try
        {
            if (categoryId <= 0)
                throw new BusinessException("CategoryId inválido.");


            var entities = await _productRepo.GetByCategoryAsync(categoryId);

            var ordered = entities
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToList();

            return _mapper.Map<IEnumerable<ProductSelectDto>>(ordered);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos por categoría {CategoryId}", categoryId);
            throw new BusinessException("Error al obtener productos por categoría.", ex);
        }
    }


    public async Task<IEnumerable<ProductSelectDto>> GetFeaturedAsync(int limit)
    {
        try
        {
            var products = await _productRepo.GetFeaturedAsync(limit);
            return _mapper.Map<List<ProductSelectDto>>(products);
           
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos destacados");
            throw new BusinessException("Error al obtener productos destacados.", ex);
        }
    }

}
