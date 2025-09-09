using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Order.Create
{
    /// <summary>
    /// DTO para crear una Orden (multipart/form-data).
    /// Requiere comprobante de pago como imagen.
    /// </summary>
    public class OrderCreateDto
    {
        public int ProductId { get; set; }
        public int QuantityRequested { get; set; }

        // Comprobante (obligatorio)
        public IFormFile? PaymentImage { get; set; }

        // Datos de entrega
        public string RecipientName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public int CityId { get; set; }
        public string? AdditionalNotes { get; set; }
    }
}
