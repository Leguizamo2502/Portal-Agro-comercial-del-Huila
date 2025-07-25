
using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Farms;
using Business.Repository;
using CloudinaryDotNet;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Select;
using Entity.DTOs.Producer.Producer.Create;
using MapsterMapper;
using Utilities.Exceptions;

namespace Business.Services.Producers.Farms
{
    public class FarmService : BusinessGeneric<FarmRegisterDto, FarmSelectDto, Farm>, IFarmService
    {

        private readonly IFarmRepository _farmRepository;
        private readonly IRolUserRepository _rolUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly ICloudinaryService _cloudinaryService;
        public FarmService(IDataGeneric<Farm> data,
                           IMapper mapper,
                           IFarmRepository farmRepository,
                           IRolUserRepository rolUserRepository,
                           IUserRepository userRepository,
                           IProducerRepository producerRepository,
                           ICloudinaryService cloudinaryService
                            ) : base(data, mapper)
        {
            _farmRepository = farmRepository;
            _rolUserRepository = rolUserRepository;
            _userRepository = userRepository;
            _producerRepository = producerRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<FarmSelectDto> RegisterWithProducer(ProducerWithFarmRegisterDto dto,int userId)
        {
            try
            {
                // 1. Verificación del usuario
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new BusinessException("Usuario no encontrado");

                if (user.Producer != null)
                    throw new BusinessException("El usuario ya es productor");

                // 2. Crear y persistir el productor
                var producer = _mapper.Map<Producer>(dto);
                producer.User = user;
                producer.Code = "PENDIENTE";

                producer = await _producerRepository.AddAsync(producer);
                await _rolUserRepository.AsignateRolProducer(user);

                // 3. Crear y guardar finca (sin imágenes aún)
                var farm = _mapper.Map<Farm>(dto);
                farm.ProducerId = producer.Id;

                farm = await _farmRepository.AddAsync(farm); // farm.Id ya tiene valor

                // 4. Subir imágenes y asignarlas
                var images = await _cloudinaryService.UploadFarmImagesAsync(dto.Images, farm.Id);
                farm.FarmImages = images;

                await _farmRepository.UpdateAsync(farm); // para persistir las imágenes

                // 5. Retornar DTO final
                return _mapper.Map<FarmSelectDto>(farm);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al registrar la finca con el productor", ex);
            }
        }


        public override async Task<FarmRegisterDto> CreateAsync(FarmRegisterDto dto)
        {
            try
            {

                var farm = _mapper.Map<Farm>(dto);

                // Guardar primero para obtener el Id
                farm = await _farmRepository.AddAsync(farm);

                // Subir imágenes y asignarlas directamente
                var images = await _cloudinaryService.UploadFarmImagesAsync(dto.Images, farm.Id);
                farm.FarmImages = images;

                // Guardar las imágenes: solo necesario si AddAsync no tiene tracking de colección
                await _farmRepository.UpdateAsync(farm); // ← Si usas EF con tracking automático, esta puede ser innecesaria

                return _mapper.Map<FarmRegisterDto>(farm);
            }
            catch (Exception ex)
            {

                throw new BusinessException("No se pudo crear la finca. Verifica los archivos o la conexión con Cloudinary.", ex);
            }

        }



        public override async Task<IEnumerable<FarmSelectDto>> GetAllAsync()
        {
            var farms = await _farmRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FarmSelectDto>>(farms);
        }




    }
}
