using Entity.DTOs.BaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Security.Selects.RolFormPermission
{
    public class RolFormPermissionSelectDto : BaseDto
    {
        public int RolId { get; set; }
        public string RolName { get; set; }
        public string FormId { get; set; }
        public int FormName { get; set; }
        public string PermissionId { get; set; }
        public int PermissionName { get; set; }
    }
}
