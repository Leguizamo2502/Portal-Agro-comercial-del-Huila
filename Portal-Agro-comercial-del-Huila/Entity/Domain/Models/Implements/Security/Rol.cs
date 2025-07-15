using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Security
{
    public class Rol : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public List<RolUser> RolUsers { get; set; } = new();
        
    }
}
