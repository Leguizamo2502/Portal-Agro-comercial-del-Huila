using Data.Interfaces.Implements.Auth;
using Data.Repository;
using Entity.Domain.Models.Implements.Auth;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Auth
{
    public class PersonRepository : DataGeneric<Person>, IPersonRepository
    {
        public PersonRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Person?> GetByUserIdAsync(int userId)
        {
            return await _dbSet 
                .FirstOrDefaultAsync(p => p.User.Id == userId && p.IsDeleted == false);
        }
    }
}
