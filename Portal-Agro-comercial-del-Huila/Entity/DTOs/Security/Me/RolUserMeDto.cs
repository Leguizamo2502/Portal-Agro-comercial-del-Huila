using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Security.Me
{
    public class RolUserMeDto
    {
        public int RolId { get; set; }
        public string RolName { get; set; }
        public List<RolPermissionMeDto> Permissions { get; set; }
    }
}
