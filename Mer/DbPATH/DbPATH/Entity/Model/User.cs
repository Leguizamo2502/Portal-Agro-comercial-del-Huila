using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        
        public bool Active { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public int PersonId { get; set; }

        //public Producer? Producer { get; set; }
        //public Consumer? Consumer { get; set; }

        public Person Person { get; set; }

        public List<RolUser> RolUser { get; set; } = new List<RolUser>();
    }
}
