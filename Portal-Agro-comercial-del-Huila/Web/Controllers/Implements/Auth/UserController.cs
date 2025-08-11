using Business.Interfaces.Implements.Auth;
using Entity.DTOs.Auth;
using Entity.DTOs.Auth.User;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Auth
{
    public class UserController : BaseController<UserDto, UserSelectDto, IUserService>
    {
        public UserController(IUserService service, ILogger<UserController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(UserDto dto)
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

        protected override async Task<IEnumerable<UserSelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task<UserSelectDto?> GetByIdAsync(int id)
        {
            return await (_service.GetByIdAsync(id));
        }

        protected override async Task<bool> UpdateAsync(int id, UserDto dto)
        {
            return await _service.UpdateAsync(dto);
        }
    }
}
