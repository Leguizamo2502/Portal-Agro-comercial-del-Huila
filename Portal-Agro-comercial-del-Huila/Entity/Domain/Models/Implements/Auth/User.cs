using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Security;

namespace Entity.Domain.Models.Implements.Auth
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        //public bool Active { get; set; } = true;

        // Clave foránea obligatoria
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public ICollection<RolUser> RolUsers { get; set; } = [];
    }
}
