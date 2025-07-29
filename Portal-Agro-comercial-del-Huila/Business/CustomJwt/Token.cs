using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Custom.Encripter;
using Data.Interfaces.Implements.Auth;
using Entity.DTOs.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Utilities.Exceptions;

namespace Business.CustomJwt
{
    public class Token : IToken
    {

        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userData;
        private readonly IRolUserRepository _rolUserData;
        private readonly ILogger<Token> _logger;
        public Token(IConfiguration configuration, IUserRepository userData, IRolUserRepository rolUserData,
            ILogger<Token> logger)
        {
            _configuration = configuration;
            _userData = userData;
            _rolUserData = rolUserData;
            _logger = logger;
        }
        public async Task<string> GenerateToken(LoginUserDto dto)
        {
            dto.Password = EncriptePassword.EncripteSHA256(dto.Password);
            var user =  await _userData.LoginUser(dto);
            var roles = await GetRolesUserAsync(user.Id);


            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, dto.Email!)
            };

            // Agregar roles al token   
            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }



            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            //var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);


            //  Crear detalles del Token
            var jwtConfig = new JwtSecurityToken
            (
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:exp"])),
                signingCredentials: credentials

            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);


        }

        public async Task<IEnumerable<string>> GetRolesUserAsync(int idUser)
        {
            try
            {
                var roles = await _rolUserData.GetRolesUserAsync(idUser);
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles del usuario con ID {UserId}", idUser);
                throw new BusinessException("Error al obtener roles del usuario", ex);
            }
        }


    }
}
