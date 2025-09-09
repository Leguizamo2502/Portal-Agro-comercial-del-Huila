using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Order.Select
{
    public class OrderListItemDto : BaseDto
    {
        //public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public int QuantityRequested { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = null!;
        public string? PaymentImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
