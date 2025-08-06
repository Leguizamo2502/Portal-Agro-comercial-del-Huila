using Business.Interfaces.IBusiness;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.Rols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Implements.Security
{
    public interface IFormService : IBusiness<FormRegisterDto, FormSelectDto>
    {
    }
}

