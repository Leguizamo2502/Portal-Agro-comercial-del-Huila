using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Auth;
using Business.Repository;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;
using Entity.DTOs.Auth.User;
using MapsterMapper;
using Utilities.Exceptions;
using Utilities.Helpers.Business;

namespace Business.Services.AuthService
{
    public class UserService : BusinessGeneric<UserDto, UserSelectDto, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRolUserRepository _rolUserRepository;
        public UserService(IDataGeneric<User> data, IMapper mapper, IUserRepository userRepository, IRolUserRepository rolUserRepository) : base(data, mapper)
        {
            _userRepository = userRepository;
            _rolUserRepository = rolUserRepository;
        }


        public override async Task<IEnumerable<UserSelectDto>> GetAllAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();

                var result = new List<UserSelectDto>();

                foreach (var user in users)
                {
                    var dto = _mapper.Map<UserSelectDto>(user);
                    dto.Roles = await _rolUserRepository.GetRolesUserAsync(user.Id);
                    result.Add(dto);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener los registros.", ex);
            }
        }

    }
}
