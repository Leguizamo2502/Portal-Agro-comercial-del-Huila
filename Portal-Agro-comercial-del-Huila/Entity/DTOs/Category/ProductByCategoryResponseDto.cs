using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Products.Select;

namespace Entity.DTOs.Category
{
    public class ProductByCategoryResponseDto
    {
        public IReadOnlyList<ProductSelectDto> Items { get; set; } = Array.Empty<ProductSelectDto>();
    
        public IReadOnlyList<int> AppliedCategoryIds { get; set; } = Array.Empty<int>();
        public bool IncludedDescendants { get; set; }
    }
}
