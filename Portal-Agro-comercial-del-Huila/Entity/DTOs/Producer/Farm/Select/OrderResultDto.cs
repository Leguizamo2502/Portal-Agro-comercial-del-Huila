using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Producer.Farm.Select
{
    public class OrderResultDto : BaseDto
    {
        //public int Id { get; set; }

        // Producto
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public double UnitPrice { get; set; }
        public int QuantityRequested { get; set; }

        // Totales
        public double Subtotal { get; set; }
        public double DeliveryFee { get; set; } // 0 en el momento de crear
        public double Total { get; set; }

        // Estado
        public string Status { get; set; } = null!;

        // Comprobante
        public string? PaymentImageUrl { get; set; }

        // Fechas
        public DateTime CreatedAt { get; set; }
    }
}
