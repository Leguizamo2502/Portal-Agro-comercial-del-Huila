using Data.Interfaces.Implements.Auth;
using Data.Repository;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Auth
{
    public class RolUserRepository : DataGeneric<RolUser>, IRolUserRepository
    {
        public RolUserRepository(ApplicationDbContext context) : base(context)
        {
        }


        public override async Task<IEnumerable<RolUser>> GetAllAsync()
        {
            return await _dbSet
                .Include(ru => ru.User.Person)
                .Include(ru => ru.Rol)
                .ToListAsync();
        }

        public async Task<RolUser> AsignateRolDefault(User user)
        {
            var rolUser = new RolUser
            {
                UserId = user.Id,
                RolId = 2, //Consumer

            };

            _context.RolUsers.Add(rolUser);
            await _context.SaveChangesAsync();

            return rolUser;
        }

        public async Task<RolUser> AsignateRolProducer(User user)
        {
            var rolUser = new RolUser
            {
                UserId = user.Id,
                RolId = 3, //Producer

            };

            _context.RolUsers.Add(rolUser);
            await _context.SaveChangesAsync();

            return rolUser;
        }

        public async Task<IEnumerable<string>> GetRolesUserAsync(int userId)
        {
            var roles = await _dbSet
                    .Where(ru => ru.UserId == userId && !string.IsNullOrWhiteSpace(ru.Rol.Name))
                    .Select(ru => ru.Rol.Name)
                    .Distinct()
                    .ToListAsync();

            return roles;
        }
    }
}
