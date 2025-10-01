using Entity.Domain.Enums;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Location;
using Entity.Domain.Models.Implements.Producers.Products;
using System.ComponentModel.DataAnnotations;

namespace Entity.Domain.Models.Implements.Orders
{
    public class Order : BaseModel
    {
        //Seguridad
        public string Code { get; set; } = default!;   // opaco, único
        //public long OrderNumber { get; set; }
        // Relaciones
        public int UserId { get; set; }
        public int ProductId { get; set; }

        // Snapshots del producto (se llenan al crear)
        public int ProducerIdSnapshot { get; set; }
        public string ProductNameSnapshot { get; set; } = null!;
        public decimal UnitPriceSnapshot { get; set; }

        // Cantidad y estado
        public int QuantityRequested { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.PendingReview;

        // Pago (se usará después; en creación queda null)
        public string? PaymentImageUrl { get; set; }
        public DateTime? PaymentUploadedAt { get; set; }
        // Futuro (flujo nuevo):
        public DateTime? AcceptedAt { get; set; }           // cuando el productor acepte
        public DateTime? PaymentSubmittedAt { get; set; }   // cuando el comprador suba comprobante

        // Decisión del productor (para rechazo o notas al aceptar)
        public DateTime? ProducerDecisionAt { get; set; }
        public string? ProducerDecisionReason { get; set; } // si rechaza
        public string? ProducerNotes { get; set; }          // nota opcional (no económica)

        // Datos de entrega (requeridos en creación)
        public string RecipientName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public int CityId { get; set; }
        public string? AdditionalNotes { get; set; }

        // Totales (sin envío: Total = Subtotal)
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        // Confirmación del cliente (se usará más adelante)
        public DateTime? UserConfirmEnabledAt { get; set; }
        public UserReceivedAnswer UserReceivedAnswer { get; set; } = UserReceivedAnswer.None;
        public DateTime? UserReceivedAt { get; set; }

        // Autocierre y concurrencia
        public DateTime? AutoCloseAt { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navegación
        public User User { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public City City { get; set; } = null!;
    }
}
