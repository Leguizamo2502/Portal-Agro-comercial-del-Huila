using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.IBusiness;
using Entity.DTOs.Security.Create.RolUser;
using Entity.DTOs.Security.Selects.RolUser;


namespace Business.Interfaces.Implements.Security
{
    public interface IRolUserService : IBusiness<RolUserRegisterDto,RolUserSelectDto>
    {
    }
}
