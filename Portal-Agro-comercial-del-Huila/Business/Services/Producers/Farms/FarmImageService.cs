using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Farms;
using Business.Repository;
using CloudinaryDotNet;
using Data.Interfaces.Implements.Producers.Farms;
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
        private readonly ICloudinaryService _cloudinaryService;
        public FarmImageService(IDataGeneric<FarmImage> data, IMapper mapper, IFarmImageRepository farmImageRepository, ICloudinaryService cloudinary) : base(data, mapper)
        {
            _farmImageRepository = farmImageRepository;
            _cloudinaryService = cloudinary;
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var image = await _farmImageRepository.GetByIdAsync(imageId);
            if (image == null)
                throw new BusinessException("La imagen no existe.");

            var publicId = _cloudinaryService.ExtractPublicId(image.ImageUrl);
            await _cloudinaryService.DeleteImageAsync(publicId);

            await _farmImageRepository.DeleteAsync(image.Id);
        }

        


    }
}
