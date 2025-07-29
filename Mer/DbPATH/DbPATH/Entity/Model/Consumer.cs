using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Consumer
    {
        public int Id { get; set; }
        public string Shipping_Address { get; set; }
        public DateTime CreateAt { get; set; }

        public List<Favorite> Favorite { get; set; } = new List<Favorite>();
        public List<Review> Review { get; set; } = new List<Review>();
        public List<Order> Order { get; set; } = new List<Order>();


    }
}
