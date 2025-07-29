using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Producer.Categories
{
    public class CategoryRegisterDto : BaseDto
    {
        public string Name { get; set; }
        public int? ParentId { get; set; } // null si es una categoría padre
    }
}
