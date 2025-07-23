using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Producer.Farm.Create
{
    public class FarmRegisterDto
    {
        public string Name { get; set; }
        public double Hectares { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<IFormFile> Images { get; set; } = new();

        public int CityId { get; set; }
        public int ProducerId { get; set; } // ID del productor ya existente
    }
}
