using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public int ProductId {  get; set; }

        public Category Category { get; set; }
        public Product Product { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
