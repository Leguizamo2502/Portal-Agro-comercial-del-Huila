using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Entity.DTOs.Security.Me
{
    public class RolPermissionMeDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public FormMeDto Form { get; set; }
    }
}
