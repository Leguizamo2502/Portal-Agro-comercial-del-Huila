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
        Task<User?> GetUserWithPersonAsync(int userId);
        Task<IEnumerable<RolUser>> GetUserRolesWithPermissionsAsync(int userId);
        Task<IEnumerable<Form>> GetFormsWithModulesByIdsAsync(List<int> formIds);

    }
}
