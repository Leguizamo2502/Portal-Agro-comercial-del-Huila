using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Auth;

namespace Entity.Domain.Models.Implements.Producers
{
    public class Producer : BaseModel
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Farm> Farms { get; set; } = [];

    }
}
