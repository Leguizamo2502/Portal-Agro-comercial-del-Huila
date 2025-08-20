using Business.Interfaces.IBusiness;
using Entity.DTOs.Products.Create;
using Entity.DTOs.Products.Select;
using Entity.DTOs.Products.Update;

namespace Business.Interfaces.Implements.Producers.Products
{
    public interface IProductService : IBusiness<ProductCreateDto,ProductSelectDto>
    {
        Task<IEnumerable<ProductSelectDto>> GetByProducer(int producerId);
        Task<ProductSelectDto> CreateProductAsync(ProductCreateDto dto);
        Task<ProductSelectDto> UpdateProductAsync(ProductUpdateDto dto);
        Task<bool> AddFavoriteAsync(int userId, int productId);
        Task<bool> RemoveFavoriteAsync(int userId, int productId);
        Task<IEnumerable<ProductSelectDto>> GetAllForUsersAsync(int userId);
        Task<IEnumerable<ProductSelectDto>> GetFavoritesForUsersAsync(int userId);
    }
}
