using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Producers
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
                .Include(f => f.FarmImages)
                .ToListAsync();
        }

    }
}
