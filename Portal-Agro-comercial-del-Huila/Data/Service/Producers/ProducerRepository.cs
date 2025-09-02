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
    public class ProducerRepository : DataGeneric<Producer>, IProducerRepository
    {
        public ProducerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Producer?> GetByCodeProducer(string codeProducer)
        {
            return await _dbSet
                .Include(p=>p.User)
                    .ThenInclude(u=> u.Person)
                .FirstOrDefaultAsync(p => p.Code == codeProducer);
        }

        public async Task<int?> GetIdProducer(int userId)
        {
            return await _dbSet
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

        }


    }
}
