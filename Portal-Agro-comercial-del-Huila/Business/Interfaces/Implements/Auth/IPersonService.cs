using Business.Interfaces.IBusiness;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;

namespace Business.Interfaces.Implements.Auth
{
    public interface IPersonService : IBusiness<PersonRegisterDto,PersonSelectDto>
    {
        Task<PersonSelectDto?> GetDataBasic(int personId);

    }
}
