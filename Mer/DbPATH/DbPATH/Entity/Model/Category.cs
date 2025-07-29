using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductCategory> ProductCategory { get; set; } = new List<ProductCategory>();
        public List<SubCategory> SubCategory { get; set; } = new List<SubCategory>();


    }
}
