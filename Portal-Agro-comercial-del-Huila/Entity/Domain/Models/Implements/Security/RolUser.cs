using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Auth;

namespace Entity.Domain.Models.Implements.Security
{
    public class RolUser : BaseModel
    {
        //public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
