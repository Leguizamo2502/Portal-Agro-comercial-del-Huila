using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Auth;

namespace Business.Interfaces.Implements
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterUserDto dto);
    }
}
