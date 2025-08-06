using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Security
{
    public class FormModule : BaseModel
    {
        public int FormId { get; set; }
        public Form Form { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}
