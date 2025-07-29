using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Review
    {
        public int Id { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int ConsumerId { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
        public Consumer Consumer { get; set; }
    }
}
