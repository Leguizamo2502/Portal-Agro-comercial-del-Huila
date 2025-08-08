using Data.Interfaces.Implements.Location;
using Data.Interfaces.Implements.Security;
using Data.Repository;
using Entity.Domain.Models.Implements.Location;
using Entity.Domain.Models.Implements.Security;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Location
{
    public class CityRepository : DataGeneric<City>, ICityRepository
    {
        public CityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Department)
                //.Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCityByDepartment(int idDepartment)
        {
            return await _dbSet
                .Include(c => c.Department) 
                .Where(c => c.DepartmentId == idDepartment && !c.IsDeleted)
                .ToListAsync();
        }
    }
}
