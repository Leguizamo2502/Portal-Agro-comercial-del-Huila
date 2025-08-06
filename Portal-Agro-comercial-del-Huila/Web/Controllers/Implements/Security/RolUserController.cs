using Business.Interfaces.Implements.Security;
using Entity.DTOs.Security.Create.RolUser;
using Entity.DTOs.Security.Selects.RolUserDto;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Security
{
    public class RolUserController : BaseController<RolUserRegisterDto, RolUserSelectDto, IRolUserService>
    {
        public RolUserController(IRolUserService service, ILogger<RolUserController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(RolUserRegisterDto dto)
        {
            await _service.CreateAsync(dto);
        }

        protected override async Task<bool> DeleteAsync(int id)
        {
            return await _service.DeleteAsync(id);
        }

        protected override async Task<bool> DeleteLogicAsync(int id)
        {
            return await _service.DeleteLogicAsync(id);
        }

        protected override async Task<IEnumerable<RolUserSelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task<RolUserSelectDto?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        protected override async Task<bool> UpdateAsync(int id, RolUserRegisterDto dto)
        {
            return await _service.UpdateAsync(dto);
        }
    }
}
