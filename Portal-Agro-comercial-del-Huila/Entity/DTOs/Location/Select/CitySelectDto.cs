using Entity.DTOs.BaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Location.Select
{
    public class CitySelectDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }

    }
}
