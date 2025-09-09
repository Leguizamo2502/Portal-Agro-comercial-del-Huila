using Entity.DTOs.BaseDTO;
using Entity.DTOs.Producer.Farm.Select;

namespace Entity.DTOs.Products.Select
{
    public class ProductSelectDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string Production { get; set; }
        public int Stock { get; set; }
        public bool Status { get; set; }
        public bool ShippingIncluded { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductImageSelectDto> Images { get; set; } = new();
        public string PersonName { get; set; }
        public string ProducerCode { get; set; }
        public string CityName { get; set; }
        public string DepartmentName { get; set; }

        // Legacy (mantener por compatibilidad con vistas actuales)
        public int FarmId { get; set; }
        public string FarmName { get; set; }

        // NUEVO: todas las fincas activas asociadas
        public List<int> FarmIds { get; set; } = new();

        public bool IsFavorite { get; set; }
        
    }
}
