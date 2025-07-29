using Business.Interfaces.Implements.Security;
using Business.Repository;
using Data.Interfaces.Implements.Security;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.Rols;
using MapsterMapper;

namespace Business.Services.Security
{
    public class RolService : BusinessGeneric<RolRegisterDto, RolSelectDto, Rol>, IRolService
    {
        private readonly IRolRepository _rolRepository;
        public RolService(IDataGeneric<Rol> data, IMapper mapper, IRolRepository rolRepository) : base(data, mapper)
        {
            _rolRepository = rolRepository;
        }
    }
}
