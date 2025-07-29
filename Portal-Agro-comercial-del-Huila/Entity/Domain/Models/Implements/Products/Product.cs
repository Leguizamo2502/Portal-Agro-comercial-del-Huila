using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Producers;

namespace Entity.Domain.Models.Implements.Products
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Production { get; set; }
        public int Stock { get; set; }
        public bool Status { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public Farm Farm { get; set; }
        public int FarmId { get; set; }
    }
}
