using System.Security.Claims;
using Business.Interfaces.Implements.Auth;
using Business.Interfaces.Implements.Security.Mes;
using Entity.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Auth
{
    public class PersonController : BaseController<PersonRegisterDto, PersonSelectDto, IPersonService>
    {
        public PersonController(IPersonService service, ILogger<PersonController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(PersonRegisterDto dto)
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

        protected override async Task<IEnumerable<PersonSelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task<PersonSelectDto?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        protected override async Task<bool> UpdateAsync(int id, PersonRegisterDto dto)
        {
            return await _service.UpdateAsync(dto);
        }


        


    }
}
