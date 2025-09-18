using Business.Interfaces.Implements;
using Data.Interfaces.Implements.Producers;
using Entity.DTOs.Producer.Producer.Select;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services.Producers
{
    public class ProducerService : IProducerService
    {
        private readonly IProducerRepository _producerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProducerService> _logger;

        public ProducerService(IProducerRepository producerRepository,
            IMapper mapper, ILogger<ProducerService> logger)
        {
            _mapper = mapper;
            _producerRepository = producerRepository;
            _logger = logger;
        }


        public async Task<ProducerSelectDto?> GetByCodeProducer(string codeProducer)
        {
            try
            {
                //if (id <= 0) throw new BusinessException("El ID debe ser mayor que cero.");
                var entity = await _producerRepository.GetByCodeProducer(codeProducer);
                return entity is null ? null : _mapper.Map<ProducerSelectDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con ID {Id}", codeProducer);
                throw new BusinessException($"Error al obtener el producto con ID {codeProducer}.", ex);
            }
        }

        public async Task<string?> GetCodeProducer(int userId)
        {
            
                var producerId =await _producerRepository.GetIdProducer(userId);
                if (producerId == null) 
                    throw new BusinessException($"No se encontró el productor asociado al usuario con ID {userId}.");
                var code = await _producerRepository.GetCodeProducer(producerId.Value);
                return code;
            
        }

        public Task<int> SalesNumberByCode(string codeProducer)
        {
            var count = _producerRepository.SalesNumberByCode(codeProducer);
            if (count == null) 
                throw new BusinessException($"No se encontró el productor con código {codeProducer}.");
            return count;
        }



    }
}
