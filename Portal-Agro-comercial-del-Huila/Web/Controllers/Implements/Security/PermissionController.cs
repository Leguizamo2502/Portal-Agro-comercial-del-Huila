using Business.Interfaces.Implements.Security;
using Entity.DTOs.Security.Create.NewFolder;
using Entity.DTOs.Security.Create.Permissions;
using Entity.DTOs.Security.Selects.Module;
using Entity.DTOs.Security.Selects.Permissions;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Security
{
    public class PermissionController : BaseController<PermissionRegisterDto, PermissionSelectDto, IPermissionService>
    {
        public PermissionController(IPermissionService service, ILogger<PermissionController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(PermissionRegisterDto dto)
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

        protected override async Task<IEnumerable<PermissionSelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task<PermissionSelectDto?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        protected override async Task<bool> UpdateAsync(int id, PermissionRegisterDto dto)
        {
            return await _service.UpdateAsync(dto);
        }
    }
}