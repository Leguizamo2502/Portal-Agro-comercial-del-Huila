using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Producer.Producer.Create
{
    public class ProducerWithFarmRegisterDto
    {
        // Info del nuevo productor
        public string Description { get; set; } // Descripción del productor (si aplica)

        // Info de la finca
        public string Name { get; set; }
        public double Hectares { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Datos clave
        public int CityId { get; set; }
        //public int UserId { get; set; } // para crear el producer asociado

    }
}
