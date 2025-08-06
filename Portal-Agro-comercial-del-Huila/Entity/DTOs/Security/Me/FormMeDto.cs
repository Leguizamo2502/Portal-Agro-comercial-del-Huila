using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Security.Me
{
    public class FormMeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Route { get; set; }
        //public List<MenuModuleDto> Modules { get; set; }
        public IEnumerable<string> Permissions { get; set; } = [];

    }
}
