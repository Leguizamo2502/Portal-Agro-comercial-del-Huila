using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Category
{
    public class ProductByCategoryRequestDto
    {
        public List<int> CategoryIds { get; set; } = new();

        public bool IncludeDescendants { get; set; } = true;
    }
}
