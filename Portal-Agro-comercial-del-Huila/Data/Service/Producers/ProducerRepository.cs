using Data.Interfaces.Implements.Producers;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.DTOs.Order.Select;
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
                .Include(p => p.User)
                    .ThenInclude(u => u.Person)
                .FirstOrDefaultAsync(p => p.Code == codeProducer);
        }

        public async Task<int?> GetIdProducer(int userId)
        {
            return await _dbSet
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

        }

        public async Task<ContactDto> GetContactProducer(int producerId)
        {
            var p = await _dbSet
                .AsNoTracking()
                .Include(p => p.User)
                    .ThenInclude(u => u.Person)
                .FirstOrDefaultAsync(p => p.Id == producerId && !p.IsDeleted);
            if (p is null)
                throw new InvalidOperationException($"No se encontró el productor con ID {producerId}.");

            return new ContactDto
            {
                FirstName = p.User.Person.FirstName,
                Email = p.User.Email,
                LastName = p.User.Person.LastName,
            };


        }


    }
}
