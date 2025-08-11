using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.IBusiness;
using Entity.DTOs.Auth;
using Entity.DTOs.Auth.User;

namespace Business.Interfaces.Implements.Auth
{
    public interface IUserService : IBusiness<UserDto,UserSelectDto>
    {
    }
}
