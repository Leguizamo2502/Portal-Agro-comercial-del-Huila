using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Farms;
using Business.Repository;
using CloudinaryDotNet;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Select;
using MapsterMapper;
using Utilities.Exceptions;

namespace Business.Services.Producers.Farms
{
    public class FarmImageService : BusinessGeneric<FarmImageDto, FarmImageDto, FarmImage>, IFarmImageService
    {
        private readonly IFarmImageRepository _farmImageRepository;
        private readonly ICloudinaryService _cloudinary;
        public FarmImageService(IDataGeneric<FarmImage> data, IMapper mapper, IFarmImageRepository farmImageRepository, ICloudinaryService cloudinary) : base(data, mapper)
        {
            _farmImageRepository = farmImageRepository;
            _cloudinary = cloudinary;
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var image = await _farmImageRepository.GetByIdAsync(imageId);
            if (image == null)
                throw new BusinessException("La imagen no existe.");

            var publicId = ExtractPublicId(image.ImageUrl);
            await _cloudinary.DeleteImageAsync(publicId);

            await _farmImageRepository.DeleteAsync(image.Id);
        }

        private string ExtractPublicId(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var path = uri.AbsolutePath; // /djj163sc9/image/upload/v1753390824/farms/4/img_<guid>.png

            // Encuentra la parte después de "/upload/"
            var uploadIndex = path.IndexOf("/upload/");
            if (uploadIndex == -1)
                throw new BusinessException("La URL no tiene el formato esperado de Cloudinary.");

            var startIndex = uploadIndex + "/upload/".Length;
            var publicIdWithVersion = path.Substring(startIndex); // v1753390824/farms/4/img_<guid>.png

            // Quita el prefijo de versión (v...) si lo hay
            var segments = publicIdWithVersion.Split('/', 2);
            if (segments.Length < 2)
                throw new BusinessException("La URL no contiene un publicId válido.");

            var publicIdWithExtension = segments[1]; // farms/4/img_<guid>.png

            // Quitar extensión
            var publicId = Path.Combine(Path.GetDirectoryName(publicIdWithExtension) ?? "",
                                        Path.GetFileNameWithoutExtension(publicIdWithExtension))
                                        .Replace("\\", "/"); // Garantizar formato UNIX

            return publicId;
        }


    }
}
