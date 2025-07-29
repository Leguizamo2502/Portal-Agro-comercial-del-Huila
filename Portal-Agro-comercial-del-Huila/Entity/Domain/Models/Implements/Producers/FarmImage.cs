using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Producers
{
    public class FarmImage : BaseModel
    {
        public string ImageUrl { get; set; }
        public int FarmId { get; set; }

        public Farm Farm { get; set; }
    }
}
