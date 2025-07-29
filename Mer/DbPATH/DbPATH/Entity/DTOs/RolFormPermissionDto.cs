using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RolFormPermissionDto
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public string RolName { get; set; }
        public int FormId { get; set; }
        public string FormName { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool IsDeleted { get; set; }

    }
}
