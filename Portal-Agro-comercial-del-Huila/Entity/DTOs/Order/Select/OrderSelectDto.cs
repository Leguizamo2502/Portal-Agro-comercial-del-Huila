using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Order.Select
{
    public class OrderSelectDto : BaseDto
    {
        public string ProductName { get; set; } = null!;
        public int QuantityRequested { get; set; }
        public decimal Subtotal { get; set; }
        public string Status { get; set; } = null!;
        public string? PaymentImageUrl { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
