using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Producers.Farms;
using Business.Repository;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Select;
using Entity.DTOs.Producer.Producer.Create;
using MapsterMapper;
using Microsoft.AspNetCore.Hosting;
using Utilities.Exceptions;

namespace Business.Services.Producers.Farms
{
    public class FarmService : BusinessGeneric<FarmRegisterDto, FarmSelectDto, Farm>, IFarmService
    {

        private readonly IFarmRepository _farmRepository;
        private readonly IRolUserRepository _rolUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly Cloudinary _cloudinary;
        public FarmService(IDataGeneric<Farm> data,
                           IMapper mapper,
                           IFarmRepository farmRepository,
                           IRolUserRepository rolUserRepository,
                           IUserRepository userRepository,
                           IProducerRepository producerRepository,
                           Cloudinary cloudinary
                            ) : base(data, mapper)
        {
            _farmRepository = farmRepository;
            _rolUserRepository = rolUserRepository;
            _userRepository = userRepository;
            _producerRepository = producerRepository;
            _cloudinary = cloudinary;

        }

        public async Task<FarmSelectDto> RegisterWithProducer(ProducerWithFarmRegisterDto dto,int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new BusinessException("Usuario no encontrado");

                if (user.Producer != null)
                    throw new BusinessException("El usuario ya es Prodcutor");

                var producer = _mapper.Map<Producer>(dto);
                producer.User = user;
                producer.Code = "PENDIENTE"; // Generar código temporal

                await _producerRepository.AddAsync(producer); // Aquí se debe generar el Id

                await _rolUserRepository.AsignateRolProducer(user);

                var farm = _mapper.Map<Farm>(dto);
                farm.Producer = producer;
                farm.ProducerId = producer.Id;
                await _farmRepository.AddAsync(farm);
                

                var farmCreated = _mapper.Map<FarmSelectDto>(farm);
                return farmCreated;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al registrar la finca con el productor", ex);
            }
        }


        public async Task<bool> CreateFarm(FarmRegisterDto dto)
        {
            try
            {
                if (dto.Images == null || dto.Images.Count == 0)
                    throw new BusinessException("Debe subir al menos una imagen.");

                if (dto.Images.Count > 5)
                    throw new BusinessException("Solo se permiten hasta 5 imágenes por finca.");

                var farm = _mapper.Map<Farm>(dto);
                farm.FarmImages = new List<FarmImage>();

                foreach (var file in dto.Images)
                {
                    if (file.Length <= 0)
                        continue;

                    var fileName = $"farm_{farm.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var uploadParams = new ImageUploadParams
                    {
                        PublicId = $"aspimage/{fileName}",
                        File = new FileDescription(file.FileName, file.OpenReadStream())
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.Error != null)
                        throw new Exception($"Error al subir la imagen: {uploadResult.Error.Message}");

                    farm.FarmImages.Add(new FarmImage
                    {
                        ImageUrl = uploadResult.SecureUrl.ToString()
                    });
                }

                await _farmRepository.AddAsync(farm);
                return true;
            }
            catch (Exception ex)
            {

                throw new BusinessException("No se pudo crear la finca. Verifica los archivos o la conexión con Cloudinary.", ex);
            }

        }

    }
}
