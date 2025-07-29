using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Producer.Farm.Select
{
    public class FarmSelectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CityName { get; set; }
        public string DepartmentName { get; set; }
        public string ProducerName { get; set; }
        public List<FarmImageDto> Images { get; set; }
        
    }
}
