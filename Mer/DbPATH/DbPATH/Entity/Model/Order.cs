using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Order
    {
        public int Id { get; set; }
        
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime OrderDate { get; set; }

        public int ConsumerId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Consumer Consumer { get; set; }
    }
}
