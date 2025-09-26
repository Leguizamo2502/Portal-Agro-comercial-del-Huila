using Entity.DTOs.Products.Select;

namespace Business.Interfaces.Implements.Producers.Products
{
    public interface IProductReadService
    {
        Task<IEnumerable<ProductSelectDto>> GetAllAsync();
        Task<ProductSelectDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductSelectDto>> GetFavoritesForUserAsync(int userId);
        Task<IEnumerable<ProductSelectDto>> GetByProducerAsync(int userId);
        Task<IEnumerable<ProductSelectDto>> GetAllHomeAsync(int? limit);

        Task<IEnumerable<ProductSelectDto>> GetByProducerCodeAsync(string codeProducer);

        Task<IEnumerable<ProductSelectDto>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductSelectDto>> GetFeaturedAsync(int limit);
        Task<ProductSelectDto?> GetDetailProduct(int? userId, int productId);
    }
}
