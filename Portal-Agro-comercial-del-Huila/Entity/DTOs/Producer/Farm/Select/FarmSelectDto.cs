using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Producer.Farm.Select
{
    public class FarmSelectDto : BaseDto
    {
        public string Name { get; set; }
        public double Hectares { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public string ProducerName { get; set; }
        public int ProducerId { get; set; }
        public List<FarmImageSelectDto> Images { get; set; }
        
    }
}
