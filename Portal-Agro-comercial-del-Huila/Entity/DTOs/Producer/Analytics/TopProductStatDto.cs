namespace Entity.DTOs.Producer.Analytics
{
    public class TopProductStatDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public int CompletedOrders { get; set; }
        public int TotalUnits { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
