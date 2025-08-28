using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Producers.Products;

namespace Data.Interfaces.Implements.Producers.Products
{
    public interface IProductRepository : IDataGeneric<Product>
    {
        Task<IEnumerable<Product>> GetByProducer(int? producerId);
        Task<IEnumerable<Product>> GetByIdsFavoritesAsync(IEnumerable<int> ids);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetByCategoriesAsync(List<int> categoryIds, bool includeDescendants);
    }
}
