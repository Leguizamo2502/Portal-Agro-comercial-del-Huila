using Business.Interfaces.IBusiness;
using Entity.DTOs.Products;

namespace Business.Interfaces.Implements.Producers.Products
{
    public interface IProductImageService : IBusiness<ProductImageSelectDto,ProductImageSelectDto>
    {
        //Task DeleteImageAsync(int imageId);
        Task DeleteImageByIdAsync(int imageId);
    }
}
