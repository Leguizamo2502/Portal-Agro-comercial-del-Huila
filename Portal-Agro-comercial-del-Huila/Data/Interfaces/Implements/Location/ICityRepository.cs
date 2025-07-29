using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Location;

namespace Data.Interfaces.Implements.Location
{
    public interface ICityRepository : IDataGeneric<City>
    {
        Task<IEnumerable<City>> GetCityByDepartment(int idDepartment);
    }
}
