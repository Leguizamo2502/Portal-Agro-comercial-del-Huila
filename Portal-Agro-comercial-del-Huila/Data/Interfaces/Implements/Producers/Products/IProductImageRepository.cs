using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Products;

namespace Data.Interfaces.Implements.Producers.Products
{
    public interface IProductImageRepository : IDataGeneric<ProductImage>
    {
        Task AddImages(List<ProductImage> images);
        Task<List<ProductImage>> GetByProductIdAsync(int productId);
        Task<bool> DeleteByPublicIdAsync(string publicId);
        Task<bool> DeleteLogicalByPublicIdAsync(string publicId);
    }
}
