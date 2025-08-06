using Business.Interfaces.IBusiness;
using Entity.DTOs.Security.Create.FormModule;
using Entity.DTOs.Security.Create.RolFormPermission;
using Entity.DTOs.Security.Selects.FormModule;
using Entity.DTOs.Security.Selects.RolFormPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Implements.Security
{
    public interface IRolFormPermissionService : IBusiness<RolFormPermissionRegisterDto, RolFormPermissionSelectDto>
    {
    }
}
