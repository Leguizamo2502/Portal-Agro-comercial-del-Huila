
using Data.Interfaces.Implements;
using Data.Repository;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using Entity.Infrastructure.Context;

namespace Data.Service
{
    public class RolUserRepository : DataGeneric<RolUser>, IRolUserRepository
    {
        public RolUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<RolUser> AsignateRolDefault(User user)
        {
            var rolUser = new RolUser
            {
                UserId = user.Id,
                RolId = 2,

            };

            _context.RolUsers.Add(rolUser);
            await _context.SaveChangesAsync();

            return rolUser;
        }
    }
}
