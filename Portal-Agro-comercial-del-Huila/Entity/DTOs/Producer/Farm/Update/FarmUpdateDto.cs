using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.BaseDTO;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Producer.Farm.Update
{
    public class FarmUpdateDto : BaseDto
    {
        //public int Id { get; set; }

        public string Name { get; set; }
        public double Hectares { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int CityId { get; set; }

        public List<IFormFile>? Images { get; set; } // Imágenes nuevas que se deseen subir

        public List<string>? ImagesToDelete { get; set; }
    }
}
