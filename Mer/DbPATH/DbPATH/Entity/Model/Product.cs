using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Production { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreateAt { get; set; }
        
        public int FarmId { get; set; }
        public Farm Farm { get; set; }


        public List<ProductCategory> ProductCategory { get; set; } = new List<ProductCategory>();
        public List<Review> Review { get; set; } = new List<Review>();

        public List<Order> Order { get; set; } = new List<Order>();

        public List<Favorite> Favorite { get; set; } = new List<Favorite>();


    }
}
