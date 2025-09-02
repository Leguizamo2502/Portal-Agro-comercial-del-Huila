using Entity.Domain.Enums;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Producers.Products;

namespace Entity.Domain.Models.Implements.Orders
{
    public class Order : BaseModel
    {
        // Relaciones mínimas
        public int UserId { get; set; }
        public int ProductId { get; set; }

        // Snapshots (inmutables respecto a cambios futuros)
        public int ProducerIdSnapshot { get; set; }
        public string ProductNameSnapshot { get; set; } = null!;
        public decimal UnitPriceSnapshot { get; set; }

        // Cantidad solicitada
        public int QuantityRequested { get; set; }

        // Estado del pedido
        public OrderStatus Status { get; set; } = OrderStatus.PendingReview;

        // Comprobante (una sola imagen)
        public string? PaymentImageUrl { get; set; }
        public DateTime? PaymentUploadedAt { get; set; }

        // Decisión del productor
        public DateTime? ProducerDecisionAt { get; set; }
        public string? ProducerDecisionReason { get; set; }

        // Envío (se paga al recibir)
        public decimal DeliveryFee { get; set; } = 0m;
        public string DeliveryFeeCurrency { get; set; } = "COP";
        public DateTime? DeliveryFeeSetAt { get; set; }
        public string? DeliveryNotes { get; set; }

        // Datos de entrega inline
        public string RecipientName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public int CityId { get; set; }
        public string? AdditionalNotes { get; set; }

        // Totales
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        // Confirmación del cliente
        public DateTime? UserConfirmEnabledAt { get; set; }
        public UserReceivedAnswer UserReceivedAnswer { get; set; } = UserReceivedAnswer.None;
        public DateTime? UserReceivedAt { get; set; }

        // Autocierre
        public DateTime? AutoCloseAt { get; set; }

        // Concurrencia
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navegación
        public User User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
