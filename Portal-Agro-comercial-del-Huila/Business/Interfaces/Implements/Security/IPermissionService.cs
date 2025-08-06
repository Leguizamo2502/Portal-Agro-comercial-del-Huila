using Business.Interfaces.IBusiness;
using Entity.DTOs.Security.Create.NewFolder;
using Entity.DTOs.Security.Create.Permissions;
using Entity.DTOs.Security.Selects.Module;
using Entity.DTOs.Security.Selects.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Implements.Security
{
    public interface IPermissionService : IBusiness<PermissionRegisterDto, PermissionSelectDto>
    {
    }
}
