using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Products.Create
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Production { get; set; }
        public int Stock { get; set; }
        public bool Status { get; set; }
        public bool ShippingIncluded { get; set; }
        public int CategoryId { get; set; }
        public int ProducerId { get; set; }

        public List<IFormFile> Images { get; set; } = new();

        // Nuevo: varias fincas
        public List<int> FarmIds { get; set; } = new();
    }
}
