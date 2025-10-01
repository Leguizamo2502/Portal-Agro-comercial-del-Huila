using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Order.Select
{
    public class OrderListItemDto : BaseDto
    {
        public string ProductName { get; set; } = null!;
        public string Code { get; set; }
        public int QuantityRequested { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = null!;

        // Ahora será null al crear, solo tendrá valor si el comprador ya subió comprobante
        public string? PaymentImageUrl { get; set; }

        // Consistencia con otros DTOs
        public DateTime CreateAt { get; set; }
    }
}
