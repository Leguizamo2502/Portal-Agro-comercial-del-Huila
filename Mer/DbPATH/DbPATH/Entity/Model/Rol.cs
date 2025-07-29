using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Rol
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ?DeleteAt { get; set; }

        public List<RolUser> RolUser { get; set; } = new List<RolUser>();
        public List<RolFormPermission> RolFormPermission { get; set; } = new List<RolFormPermission>();
    }
}
