using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.IBusiness;
using Entity.DTOs.Location.Create;
using Entity.DTOs.Location.Select;

namespace Business.Interfaces.Implements.Location
{
    public interface IDepartmentService : IBusiness<DepartmentRegisterDto, DepartmentSelectDto>
    {
    }
}