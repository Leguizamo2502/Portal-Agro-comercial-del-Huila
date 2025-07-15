using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Auth;

namespace Business.CustomJwt
{
    public interface IToken
    {
        Task<string> GenerateToken(LoginUserDto dto);
        bool validarToken(string token);
    }
}
