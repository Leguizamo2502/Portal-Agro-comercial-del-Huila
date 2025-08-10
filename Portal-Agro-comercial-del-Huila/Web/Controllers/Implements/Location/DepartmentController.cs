using Business.Interfaces.Implements.Location;
using Business.Interfaces.Implements.Security;
using Entity.DTOs.Location.Create;
using Entity.DTOs.Location.Select;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Location
{
    public class DepartmentController : BaseController<DepartmentRegisterDto, DepartmentSelectDto, IDepartmentService>
    {
        public DepartmentController(IDepartmentService service, ILogger<DepartmentController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(DepartmentRegisterDto dto)
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

        protected override async Task<IEnumerable<DepartmentSelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task<DepartmentSelectDto?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        protected override async Task<bool> UpdateAsync(int id, DepartmentRegisterDto dto)
        {
            return await _service.UpdateAsync(dto);
        }
    }
}