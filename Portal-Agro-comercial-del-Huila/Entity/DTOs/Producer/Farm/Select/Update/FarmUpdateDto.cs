using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Producer.Farm.Update
{
    public class FarmUpdateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public double Hectares { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int CityId { get; set; }

        public List<IFormFile>? NewImages { get; set; } // Imágenes nuevas que se deseen subir

        public List<int>? ImagesToDelete { get; set; } // IDs de las imágenes existentes a eliminar
    }
}
