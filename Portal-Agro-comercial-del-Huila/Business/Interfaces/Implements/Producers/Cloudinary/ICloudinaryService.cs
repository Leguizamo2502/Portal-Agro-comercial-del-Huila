using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Business.Interfaces.Implements.Producers.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadFarmImagesAsync(IFormFile file, int farmid);
        Task DeleteAsync(string publicId);

        Task<ImageUploadResult> UploadProductImagesAsync(IFormFile file, int productid);
        string ExtractPublicId(string imageUrl);


    }
}
