using Business.Interfaces.IBusiness;
using Entity.DTOs.Products;

namespace Business.Interfaces.Implements.Producers.Products
{
    public interface IProductImageService : IBusiness<ProductImageDto,ProductImageDto>
    {
        Task DeleteImageAsync(int imageId);
    }
}
