using Business.Interfaces.IBusiness;
using Entity.DTOs.Security.Create.NewFolder;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.Module;
using Entity.DTOs.Security.Selects.Rols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Implements.Security
{
    public interface IModuleService : IBusiness<ModuleRegisterDto, ModuleSelectDto>
    {
    }
}
