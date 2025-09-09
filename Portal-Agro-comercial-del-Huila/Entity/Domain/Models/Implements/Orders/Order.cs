using Entity.Domain.Enums;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Producers.Products;

namespace Entity.Domain.Models.Implements.Orders
{
    public class Order : BaseModel
    {
        // Relaciones
        public int UserId { get; set; }
        public int ProductId { get; set; }

        // Snapshots del producto
        public int ProducerIdSnapshot { get; set; }
        public string ProductNameSnapshot { get; set; } = null!;
        public decimal UnitPriceSnapshot { get; set; }

        // Cantidad y estado
        public int QuantityRequested { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.PendingReview;

        // Comprobante
        public string? PaymentImageUrl { get; set; }
        public DateTime? PaymentUploadedAt { get; set; }

        // Decisión del productor
        public DateTime? ProducerDecisionAt { get; set; }
        public string? ProducerDecisionReason { get; set; } // si rechaza
        public string? ProducerNotes { get; set; }          // nota opcional al aceptar (no económica)

        // Datos de entrega
        public string RecipientName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public int CityId { get; set; }
        public string? AdditionalNotes { get; set; }

        // Totales (sin envío: Total = Subtotal)
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        // Confirmación del cliente
        public DateTime? UserConfirmEnabledAt { get; set; }
        public UserReceivedAnswer UserReceivedAnswer { get; set; } = UserReceivedAnswer.None;
        public DateTime? UserReceivedAt { get; set; }

        // Autocierre y concurrencia
        public DateTime? AutoCloseAt { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navegación
        public User User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
