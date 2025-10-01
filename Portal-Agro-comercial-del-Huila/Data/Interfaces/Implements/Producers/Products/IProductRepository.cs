using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers.Products;

namespace Data.Interfaces.Implements.Producers.Products
{
    public interface IProductRepository : IDataGeneric<Product>
    {
        Task<IEnumerable<Product>> GetByProducer(int? producerId);
        Task<IEnumerable<Product>> GetByProducerCode(string producerCode);
        Task<IEnumerable<Product>> GetByIdsFavoritesAsync(IEnumerable<int> ids);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetByCategoriesAsync(List<int> categoryIds, bool includeDescendants);
        Task<IEnumerable<Product>> GetFeaturedAsync(int limit);
        Task<IEnumerable<Product>> GetAllWithLimitAsync(int? limit);
        Task<bool> UpdateStock(int productId, int newStock);
        Task<bool> TryDecrementStockAsync(int productId, int quantity);
        Task<Product?> GetByIdSmall(int id);
    }
}
