using Business.Interfaces.Implements.Security;
using Entity.DTOs.Security.Create.FormModule;
using Entity.DTOs.Security.Create.RolFormPermission;
using Entity.DTOs.Security.Selects.FormModule;
using Entity.DTOs.Security.Selects.RolFormPermission;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Security
{
    public class RolFormPermissionController : BaseController<RolFormPermissionRegisterDto, RolFormPermissionSelectDto, IRolFormPermissionService>
    {
        public RolFormPermissionController(IRolFormPermissionService service, ILogger<RolFormPermissionController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(RolFormPermissionRegisterDto dto)
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

        protected override async Task<IEnumerable<RolFormPermissionSelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task<RolFormPermissionSelectDto?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        protected override async Task<bool> UpdateAsync(int id, RolFormPermissionRegisterDto dto)
        {
            return await _service.UpdateAsync(dto);
        }
    }
}