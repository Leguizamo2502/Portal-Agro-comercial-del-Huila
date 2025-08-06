using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.IBusiness;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Select;

namespace Business.Interfaces.Implements.Producers.Farms
{
    public interface IFarmImageService : IBusiness<FarmImageDto, FarmImageDto>
    {
        Task DeleteImageAsync(int imageId);
    }
}
