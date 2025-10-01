using Business.Interfaces.Implements;
using Data.Interfaces.Implements.Producers;
using Entity.Domain.Models.Implements.Producers;
using Entity.DTOs.Producer.Producer.Select;
using Entity.DTOs.Producer.Producer.Update;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers.Business;

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
                var entity = await _producerRepository.GetByCodeProducer(codeProducer);

                if (entity is null)
                    return null;

                var dto = _mapper.Map<ProducerSelectDto>(entity);

                dto.AverageRating = await _producerRepository.GetAverageRatingAsync(entity.Id);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el productor con code {Code}", codeProducer);
                throw new BusinessException($"Error al obtener el productor con code {codeProducer}.", ex);
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

        public async Task<bool> UpdateProfileAsync(int userId, ProducerUpdateDto dto)
        {
            try
            {
                var producerId = await _producerRepository.GetIdProducer(userId)
                    ?? throw new BusinessException("No se encontró productor para este usuario.");

                var producer = await _producerRepository.GetByIdWithSocialLinksAsync(producerId)
                    ?? throw new BusinessException("No se encontró productor por id.");

                producer.Description = dto.Description?.Trim() ?? string.Empty;

                if (producer.SocialLinks.Any())
                    _producerRepository.RemoveRange(producer.SocialLinks);

                if (dto.SocialLinks is { Count: > 0 })
                {
                    var cleaned = dto.SocialLinks
                        .Where(sl => !string.IsNullOrWhiteSpace(sl.Url))
                        .GroupBy(sl => sl.Network)
                        .Select(g => g.Last()) 
                        .ToList();

                    foreach (var sl in cleaned)
                    {
                        var url = Urls.NormalizeUrl(sl.Network, sl.Url);
                        producer.SocialLinks.Add(new ProducerSocialLink
                        {
                            Network = sl.Network,
                            Url = url
                        });
                    }
                }

                var ok = await _producerRepository.UpdateAsync(producer);
                return ok;
            }
            catch (DbUpdateException ex)
            {
                throw new BusinessException("No se pudo actualizar el perfil. Revisa que no repitas el mismo tipo de red social.", ex);
            }
            catch (BusinessException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error inesperado actualizando el perfil del productor.", ex);
            }

        }
    }
}
