using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Producers.Farms;
using Business.Repository;
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
        private readonly IWebHostEnvironment _env;
        public FarmService(IDataGeneric<Farm> data,
                           IMapper mapper,
                           IFarmRepository farmRepository,
                           IRolUserRepository rolUserRepository,
                           IUserRepository userRepository,
                           IProducerRepository producerRepository, IWebHostEnvironment env) : base(data, mapper)
        {
            _farmRepository = farmRepository;
            _rolUserRepository = rolUserRepository;
            _userRepository = userRepository;
            _producerRepository = producerRepository;
            _env = env;
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


        public async Task<int> CreateFarm(FarmRegisterDto dto)
        {
            var farm = _mapper.Map<Farm>(dto);
            string uploadPath = Path.Combine(_env.WebRootPath, "uploads", "farms");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (var image in dto.Images)
            {
                if (image.Length > 0)
                {
                    var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var fullPath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    farm.FarmImages.Add(new FarmImage
                    {
                        ImageUrl = $"/uploads/farms/{fileName}"
                    });
                }
            }

            await _farmRepository.AddAsync(farm);

            //return _mapper.Map<FarmSelectDto>(farm);
            return farm.Id;
        }

    }
}
