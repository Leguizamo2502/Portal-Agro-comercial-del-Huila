using Entity.DTOs.BaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Security.Create.RolUser
{
    public class RolUserRegisterDto : BaseDto
    {
        public int RolId { get; set; }
        public int UserId { get; set; }
    }
}
