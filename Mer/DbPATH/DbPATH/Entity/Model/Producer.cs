using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Producer
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Farm> Farm { get; set; } = new List<Farm>();

    }
}
