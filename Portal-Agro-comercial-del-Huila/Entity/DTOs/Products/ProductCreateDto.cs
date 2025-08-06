using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Products
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Production { get; set; }
        public int Stock { get; set; }
        public string Status { get; set; } = "Disponible";
        public int CategoryId { get; set; }
        public List<IFormFile> Images { get; set; } = new();

        public int FarmId { get; set; }
    }
}
