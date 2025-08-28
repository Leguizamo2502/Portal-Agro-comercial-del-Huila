using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Producers.Products
{
    public class ProductImage : BaseModel
    {
        public string FileName { get; set; } = null!;
        public string PublicId { get; set; } = null!;

        public string ImageUrl { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
