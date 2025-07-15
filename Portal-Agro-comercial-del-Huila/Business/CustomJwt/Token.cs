using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements;
using Entity.DTOs.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Utilities.Custom;

namespace Business.CustomJwt
{
    public class Token : IToken
    {

        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userData;
        private readonly IRolUserRepository _rolUserData;
        private readonly EncriptePassword _utilities;
        public Token(IConfiguration configuration, IUserRepository userData, IRolUserRepository rolUserData, EncriptePassword utilities)
        {
            _configuration = configuration;
            _userData = userData;
            _rolUserData = rolUserData;
            _utilities = utilities;

        }
        public async Task<string> GenerateToken(LoginUserDto dto)
        {
            dto.Password = _utilities.EncripteSHA256(dto.Password);
            var user =  await _userData.LoginUser(dto);


            var userClaims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, dto.Email!)
            };


            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //  Crear detalles del Token
            var jwtConfig = new JwtSecurityToken
            (
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:exp"])),
                signingCredentials: credentials

            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);


        }

        
    }
}
