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
        public string Email { get; set; }
        public PersonDto Person { get; set; }
        public List<RolUserMeDto> Roles { get; set; }
        public List<FormMeDto> Forms { get; set; }
    }
}
