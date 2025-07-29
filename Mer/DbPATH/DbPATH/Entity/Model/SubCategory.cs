using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductCategory> ProductCategory { get; set; } = new List<ProductCategory>();

    }
}
