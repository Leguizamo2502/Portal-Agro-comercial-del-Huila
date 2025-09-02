using Entity.DTOs.BaseDTO;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Products.Update
{
    public class ProductUpdateDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public bool ShippingIncluded { get; set; }
        public string Production { get; set; }
        public int Stock { get; set; }
        public bool Status { get; set; }

        //public string Status { get; set; } = "Disponible";
        public int CategoryId { get; set; }
        public List<IFormFile> Images { get; set; } = new();

        public List<int> FarmIds { get; set; } = new();
        public List<string>? ImagesToDelete { get; set; }
    }
}
