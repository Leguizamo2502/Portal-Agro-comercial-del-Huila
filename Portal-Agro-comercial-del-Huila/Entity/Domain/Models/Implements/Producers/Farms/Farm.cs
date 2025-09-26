
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Location;
using Entity.Domain.Models.Implements.Producers.Farms;

namespace Entity.Domain.Models.Implements.Producers
{
    public class Farm : BaseModel
    {
        public string Name { get; set; } = default!;
        public double Hectares { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int ProducerId { get; set; }
        public Producer Producer { get; set; } = default!;

        public int CityId { get; set; }
        public City City { get; set; } = default!;

        public List<FarmImage> FarmImages { get; set; } = new();

        // N–M vía pivote
        public ICollection<ProductFarm> ProductFarms { get; set; } = [];

    }
}
