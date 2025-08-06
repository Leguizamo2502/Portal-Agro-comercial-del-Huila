using Entity.DTOs.BaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Security.Create.NewFolder
{
    public class ModuleRegisterDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
