using Business.Interfaces.IBusiness;
using Entity.DTOs.Products.Create;
using Entity.DTOs.Products.Select;
using Entity.DTOs.Products.Update;

namespace Business.Interfaces.Implements.Producers.Products
{
    public interface IProductService : IBusiness<ProductCreateDto,ProductSelectDto>
    {
        
        Task<int> CreateProductAsync(ProductCreateDto dto);
        Task<bool> UpdateProductAsync(ProductUpdateDto dto, int userId);
        Task<bool> AddFavoriteAsync(int userId, int productId);
        Task<bool> RemoveFavoriteAsync(int userId, int productId);
        Task<bool> UpdateStockAsync(UpdateStockDto dto);

    }
}
