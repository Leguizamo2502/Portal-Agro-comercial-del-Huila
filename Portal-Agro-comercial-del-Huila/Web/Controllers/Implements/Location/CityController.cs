
using Business.Interfaces.Implements.Location;
using Entity.DTOs.Location.Create;
using Entity.DTOs.Location.Select;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Location
{
    public class CityController : BaseController<CityRegisterDto, CitySelectDto, ICityService>
    {
        public CityController(ICityService service, ILogger<CityController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(CityRegisterDto dto)
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

        protected override async Task<IEnumerable<CitySelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task<CitySelectDto?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        protected override async Task<bool> UpdateAsync(int id, CityRegisterDto dto)
        {
            return await _service.UpdateAsync(dto);
        }
    }
}
