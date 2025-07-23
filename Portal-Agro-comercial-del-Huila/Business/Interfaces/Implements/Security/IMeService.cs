using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Security.Me;

namespace Business.Interfaces.Implements.Security
{
    public interface IMeService 
    {
        Task<UserMeDto> GetAllDataMeAsync(int userId);


    }
}
