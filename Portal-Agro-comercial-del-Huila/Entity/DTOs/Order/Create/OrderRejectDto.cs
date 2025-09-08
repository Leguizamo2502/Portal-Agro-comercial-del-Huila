namespace Entity.DTOs.Order.Create
{
    public class OrderRejectDto
    {
        public string Reason { get; set; } = null!;
        public string RowVersion { get; set; } = null!; // Base64 del RowVersion
    }
}
