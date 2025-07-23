
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Location;

namespace Entity.Domain.Models.Implements.Producers
{
    public class Farm : BaseModel
    {
        public string Name { get; set; }

        public double Hectares { get; set; }

        public double Altitude { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        // Claves foráneas
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
