using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Models.Base
{
    public class BaseSecurity: BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
