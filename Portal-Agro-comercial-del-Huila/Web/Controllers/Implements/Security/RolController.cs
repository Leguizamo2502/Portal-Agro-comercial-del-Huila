using Business.Interfaces.Implements.Security;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.Rols;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Security
{
    public class RolController : BaseController<RolRegisterDto, RolSelectDto, IRolService>
    {
        public RolController(IRolService service, ILogger<RolController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(RolRegisterDto dto)
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

        protected override async Task<IEnumerable<RolSelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task<RolSelectDto?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        protected override async Task<bool> UpdateAsync(int id, RolRegisterDto dto)
        {
            return await _service.UpdateAsync(dto);
        }
    }
}
