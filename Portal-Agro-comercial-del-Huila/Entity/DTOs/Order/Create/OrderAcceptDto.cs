namespace Entity.DTOs.Order.Create
{
    public class OrderAcceptDto
    {
        public string? Notes { get; set; }
        public string RowVersion { get; set; } = null!;
    }
}
