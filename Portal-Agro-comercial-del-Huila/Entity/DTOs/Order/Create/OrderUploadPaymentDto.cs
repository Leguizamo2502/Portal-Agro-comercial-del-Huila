using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Order.Create
{
    public class OrderUploadPaymentDto
    {
        public IFormFile PaymentImage { get; set; } = null!;
        public string RowVersion { get; set; } = null!; // Base64
    }

}
