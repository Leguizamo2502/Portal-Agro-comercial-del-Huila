using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Farm
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Location { get; set; }
        public int Hectares { get; set; }
        public string ImageUrl { get; set; }
        public int Altitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Length { get; set; }

        public int ProducerId { get; set; }
        public Producer Producer { get; set; }



        public List<Product> Product { get; set; } = new List<Product>();

    }
}
