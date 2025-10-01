using Entity.DTOs.Producer.Producer.Create;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Producer.Farm.Create
{
    public class ProducerWithFarmRegisterDto
    {
        //producer
        public string Description { get; set; }
        public List<ProducerSocialCreateDto>? SocialLinks { get; set; }

        //farm
        public string Name { get; set; }
        public double Hectares { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<IFormFile> Images { get; set; }

        
        public int CityId { get; set; }
        

    }
}
