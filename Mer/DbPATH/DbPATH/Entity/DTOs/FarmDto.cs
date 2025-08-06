using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class FarmDto
    {
        public int Id { get; set; }
        public int ProducerId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Hectares { get; set; }
        public string ImageUrl { get; set; }
        public int Altitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Length { get; set; }
        
    }
}
