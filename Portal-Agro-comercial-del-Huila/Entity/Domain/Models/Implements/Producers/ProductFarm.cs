using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Producers.Products;

namespace Entity.Domain.Models.Implements.Producers
{
    public class ProductFarm : BaseModel
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int FarmId { get; set; }
        public Farm Farm { get; set; } = default!;
    }
}
