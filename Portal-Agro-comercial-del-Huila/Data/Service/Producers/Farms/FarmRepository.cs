using Data.Interfaces.Implements.Producers;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Producers.Farms
{
    public class FarmRepository : DataGeneric<Farm>, IFarmRepository
    {
        public FarmRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Farm>> GetAllAsync()
        {
            return await _dbSet
                .Include(f => f.City)
                    .ThenInclude(c => c.Department)
                .Include(f => f.Producer)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.Person)
                .Include(f => f.FarmImages)
                .ToListAsync();
        }

        public override async Task<Farm?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(f => f.City)
                    .ThenInclude(c => c.Department)
                .Include(f => f.Producer)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.Person)
                .Include(f => f.FarmImages)
                .FirstOrDefaultAsync(f=> f.Id == id);
        }

    }
}
