using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Products.Select;

namespace Business.Interfaces.Implements.Producers.Products
{
    public interface IProductReadService
    {
        Task<IEnumerable<ProductSelectDto>> GetAllAsync();
        Task<ProductSelectDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductSelectDto>> GetAllForUserAsync(int userId);
        Task<IEnumerable<ProductSelectDto>> GetFavoritesForUserAsync(int userId);
        Task<IEnumerable<ProductSelectDto>> GetByProducerAsync(int userId);

        Task<IEnumerable<ProductSelectDto>> GetByCategoryAsync(int categoryId);
    }
}
