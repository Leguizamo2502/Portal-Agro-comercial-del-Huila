using Business.Interfaces.Implements.Security;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.Rols;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Security
{
    public class FormController : BaseController<FormRegisterDto, FormSelectDto, IFormService>
    {
        public FormController(IFormService service, ILogger<FormController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(FormRegisterDto dto)
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

        protected override async Task<IEnumerable<FormSelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task<FormSelectDto?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        protected override async Task<bool> UpdateAsync(int id, FormRegisterDto dto)
        {
            return await _service.UpdateAsync(dto);
        }
    }
}
