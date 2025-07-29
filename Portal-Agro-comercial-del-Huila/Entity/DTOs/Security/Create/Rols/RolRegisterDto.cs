using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Security.Create.Rols
{
    public class RolRegisterDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
