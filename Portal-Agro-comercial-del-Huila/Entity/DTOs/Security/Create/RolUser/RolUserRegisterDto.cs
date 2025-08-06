using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Security.Create.RolUser
{
    public class RolUserRegisterDto : BaseModel
    {
        
        public int UserId { get; set; }
        public User User { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
