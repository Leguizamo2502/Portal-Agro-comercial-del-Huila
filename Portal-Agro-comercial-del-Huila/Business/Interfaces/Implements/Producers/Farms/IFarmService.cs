using Business.Interfaces.IBusiness;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Select;
using Entity.DTOs.Producer.Farm.Update;

namespace Business.Interfaces.Implements.Producers.Farms
{
    public interface IFarmService : IBusiness<FarmRegisterDto, FarmSelectDto>
    {
        Task<FarmSelectDto> RegisterWithProducer(ProducerWithFarmRegisterDto dto, int userId);
        //Task<bool> CreateFarm(FarmRegisterDto dto);

        Task<IEnumerable<FarmSelectDto>> GetByProducer(int producerId);
        Task<FarmRegisterDto> CreateFarmAsync(FarmRegisterDto dto);
        Task<FarmSelectDto> UpdateFarmAsync(FarmUpdateDto dto);
        Task<IEnumerable<FarmSelectDto>> GetByProducerCodeAsync(string codeProducer);


    }
}
