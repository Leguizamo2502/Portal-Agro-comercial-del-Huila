using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Auth;
using Microsoft.SqlServer.Server;

namespace Entity.DTOs.Security.Me
{
    public class UserMeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }



        public IEnumerable<string> Roles { get; set; } = [];
        public IEnumerable<string> Permissions { get; set; } = [];

        public IEnumerable<MenuModuleDto> Menu { get; set; } = [];

    }
}
