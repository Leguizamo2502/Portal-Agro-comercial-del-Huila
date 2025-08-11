using Entity.DTOs.Producer.Farm.Select;

namespace Entity.DTOs.Products
{
    public class ProductSelectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Production { get; set; }
        public int Stock { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        //public List<IFormFile> Images { get; set; } = new();
        public List<FarmImageDto> Images { get; set; }
        public string PersonName { get; set; }

        public int FarmId { get; set; }
        public string FarmName { get; set; }
    }
}
