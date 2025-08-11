using Entity.DTOs.BaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Location.Create
{
    public class CityRegisterDto : BaseDto
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }
    }
}
