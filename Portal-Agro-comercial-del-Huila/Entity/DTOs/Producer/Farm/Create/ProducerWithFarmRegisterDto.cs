using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Producer.Farm.Create
{
    public class ProducerWithFarmRegisterDto
    {
        
        public string Description { get; set; } 

        
        public string Name { get; set; }
        public double Hectares { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<IFormFile> Images { get; set; }

        
        public int CityId { get; set; }
        

    }
}
