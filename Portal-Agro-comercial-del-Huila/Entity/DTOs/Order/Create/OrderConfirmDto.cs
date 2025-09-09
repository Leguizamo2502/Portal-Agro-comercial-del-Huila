namespace Entity.DTOs.Order.Create
{
    public class OrderConfirmDto
    {
        // "Yes" o "No" (insensible a mayúsculas/minúsculas)
        public string Answer { get; set; } = null!;

        // RowVersion en Base64 para concurrencia
        public string RowVersion { get; set; } = null!;
    }
}
