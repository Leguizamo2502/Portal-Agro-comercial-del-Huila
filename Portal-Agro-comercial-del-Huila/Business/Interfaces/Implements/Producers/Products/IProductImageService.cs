using Business.Interfaces.IBusiness;
using Entity.DTOs.Products.Select;
using Microsoft.AspNetCore.Http;

namespace Business.Interfaces.Implements.Producers.Products
{
    public interface IProductImageService : IBusiness<ProductImageSelectDto,ProductImageSelectDto>
    {
        //Task DeleteImageAsync(int imageId);
        Task<List<ProductImageSelectDto>> GetImagesByProductIdAsync(int productId);
        Task<List<ProductImageSelectDto>> AddImagesAsync(int productId, IFormFileCollection files);
        Task DeleteImageByIdAsync(int imageId);
        Task<bool> DeleteLogicalByPublicIdAsync(string publicId);
        Task DeleteImagesByPublicIdsAsync(List<string> publicIds);


    }
}
