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
        public List<ModuleMeDto> Modules { get; set; }
    }
}
