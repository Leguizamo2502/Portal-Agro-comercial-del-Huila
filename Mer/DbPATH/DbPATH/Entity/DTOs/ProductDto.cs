using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Production { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreateAt { get; set; }
        public int FincaId { get; set; }
    }
}
