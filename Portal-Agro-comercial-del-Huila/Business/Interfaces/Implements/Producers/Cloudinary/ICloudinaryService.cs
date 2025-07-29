using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Products;
using Microsoft.AspNetCore.Http;

namespace Business.Interfaces.Implements.Producers.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<List<FarmImage>> UploadFarmImagesAsync(List<IFormFile> files, int farmid);
        Task DeleteImageAsync(string publicId);

        Task<List<ProductImage>> UploadProductImagesAsync(List<IFormFile> files, int productid);
        string ExtractPublicId(string imageUrl);

    }
}
