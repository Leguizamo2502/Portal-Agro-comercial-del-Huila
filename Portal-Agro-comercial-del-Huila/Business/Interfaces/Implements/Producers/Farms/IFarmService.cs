using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.IBusiness;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Select;
using Entity.DTOs.Producer.Producer.Create;

namespace Business.Interfaces.Implements.Producers.Farms
{
    public interface IFarmService : IBusiness<FarmRegisterDto, FarmSelectDto>
    {
        Task<FarmSelectDto> RegisterWithProducer(ProducerWithFarmRegisterDto dto, int userId);
        Task<bool> CreateFarm(FarmRegisterDto dto);

    }
}
