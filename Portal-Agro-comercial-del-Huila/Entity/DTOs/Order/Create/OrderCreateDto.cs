using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Order.Create
{
    
    public class OrderCreateDto
    {
        public int ProductId { get; set; }
        public int QuantityRequested { get; set; }

        // Datos de entrega (requeridos)
        public string RecipientName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public int CityId { get; set; }

        // Opcional
        public string? AdditionalNotes { get; set; }
    }
}
