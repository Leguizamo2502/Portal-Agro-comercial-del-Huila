using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Order.Select
{
    public class OrderDetailDto : BaseDto
    {

        public string Code { get; set; }
        // Producto / snapshots
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }

        // Cantidad y totales (sin envío)
        public int QuantityRequested { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        // Estados
        public string Status { get; set; } = null!;
        public string UserReceivedAnswer { get; set; } = "None";

        // Comprobante (aún null)
        public string? PaymentImageUrl { get; set; }
        public DateTime? PaymentUploadedAt { get; set; }

        // Decisión del productor
        public DateTime? ProducerDecisionAt { get; set; }
        public string? ProducerDecisionReason { get; set; }
        public string? ProducerNotes { get; set; }

        // Entrega
        public string RecipientName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string DepartmentName { get; set; }
        public string? AdditionalNotes { get; set; }

        // Cliente
        public DateTime? UserReceivedAt { get; set; }

        // Metadatos
        public DateTime CreateAt { get; set; }
        public string RowVersion { get; set; } = string.Empty; // Base64
    }
}
