using Entity.DTOs.BaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Security.Create.FormModule
{
    public class FormModuleRegisterDto : BaseDto
    {
        public int FormId { get; set; }
        public int ModuleId { get; set; }
    }
}
