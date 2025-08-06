using Business.Interfaces.Implements.Security;
using Business.Repository;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Security.Create.RolUser;
using Entity.DTOs.Security.Selects.RolUserDto;
using MapsterMapper;
using Utilities.Exceptions;

namespace Business.Services.Security
{
    public class RolUserService : BusinessGeneric<RolUserRegisterDto, RolUserSelectDto, RolUser>, IRolUserService
    {
        private readonly IRolUserRepository _repository;
        public RolUserService(IDataGeneric<RolUser> data, IMapper mapper, IRolUserRepository repository) : base(data, mapper)
        {
            _repository = repository;
        }

        public override async Task<IEnumerable<RolUserSelectDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<RolUserSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }
    }
}
