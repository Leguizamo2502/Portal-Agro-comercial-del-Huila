using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements;
using Data.Interfaces.Implements;
using Data.Service;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Auth;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace Business.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userData;
        private readonly IRolUserRepository _rolUserData;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userData,ILogger<AuthService> logger, IRolUserRepository rolUserData, IMapper mapper)
        {
            _logger = logger;
            _userData = userData;
            _rolUserData = rolUserData;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
            // Validar que el correo no esté registrado
            if (await _userData.ExistsByEmailAsync(dto.Email))
                throw new Exception("Correo ya registrado");

            // Mapear DTO a entidades
            var person = _mapper.Map<Person>(dto);
            var user = _mapper.Map<User>(dto);
            user.Person = person;

            // Guardar usuario
            await _userData.AddAsync(user);

            // Asignar rol por defecto
            //var rolUser = new RolUser { UserId = user.Id, RolId = 2 };
            //await _rolUserRepo.AddAsync(rolUser);
            await _rolUserData.AsignateRolDefault(user);

            return _mapper.Map<UserDto>(user);
        }


    }
}
