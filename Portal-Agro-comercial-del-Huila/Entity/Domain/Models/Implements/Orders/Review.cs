using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Producers.Products;

namespace Entity.Domain.Models.Implements.Orders
{
    public class Review : BaseModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }       // consumidor que reseña
        public byte Rating { get; set; }      // 1..5
        public string Comment { get; set; }   // requerido

        public Product Product { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
