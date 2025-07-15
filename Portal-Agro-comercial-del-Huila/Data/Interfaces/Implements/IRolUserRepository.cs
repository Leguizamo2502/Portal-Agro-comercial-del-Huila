using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;

namespace Data.Interfaces.Implements
{
    public interface IRolUserRepository : IDataGeneric<RolUser>
    {
        Task<RolUser> AsignateRolDefault(User user);
    }
}
