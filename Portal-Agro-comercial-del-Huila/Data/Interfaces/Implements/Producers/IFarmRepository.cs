using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers;
using Entity.DTOs.Producer.Farm.Select;
using Entity.DTOs.Producer.Producer.Create;

namespace Data.Interfaces.Implements.Producers
{
    public interface IFarmRepository : IDataGeneric<Farm>
    {
    }
}
