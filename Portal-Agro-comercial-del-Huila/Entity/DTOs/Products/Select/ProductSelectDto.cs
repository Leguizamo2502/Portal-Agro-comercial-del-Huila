using Entity.DTOs.BaseDTO;
using Entity.DTOs.Producer.Farm.Select;

namespace Entity.DTOs.Products.Select
{
    public class ProductSelectDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Production { get; set; }
        public int Stock { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductImageSelectDto> Images { get; set; } = new();
        public string PersonName { get; set; }
        public string CityName { get; set; }
        public string DepartmentName { get; set; }

        public int FarmId { get; set; }
        public string FarmName { get; set; }
        public bool IsFavorite { get; set; }
    }
}
