using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;

namespace Data.Interfaces.Implements.Security
{
    public interface IMeRepository
    {
        Task<User> GetUserAsync(int userId);
        Task<Person> GetPersonByUserAsync(int userId);
        Task<List<Rol>> GetRolesByUserAsync(int userId);
        Task<List<RolFormPermission>> GetPermissionsByUserAsync(int userId);

    }
}
