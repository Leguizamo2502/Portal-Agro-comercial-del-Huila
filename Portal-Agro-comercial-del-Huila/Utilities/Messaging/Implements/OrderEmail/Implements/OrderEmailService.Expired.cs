namespace Utilities.Messaging.Implements
{
    public partial class OrderEmailService
    {
        public async Task SendOrderExpiredByNoPaymentToCustomer(
            string emailReceptor,
            int orderId,
            string productName,
            int quantityRequested,
            decimal total,
            DateTime expiredAtUtc)
                {
                    var subject = $"Tu pedido #{orderId} expiró por no subir comprobante";
                    var body = $@"
        <p style='margin:0 0 16px; color:#4a5568;'>
          Tu pedido expiró porque no recibimos tu comprobante de pago a tiempo.
        </p>
        <div style='border:1px solid #e2e8f0; border-radius:8px; padding:16px; margin:16px 0;'>
          <p style='margin:6px 0;color:#4a5568;'>Pedido: <strong>#{orderId}</strong></p>
          <p style='margin:6px 0;color:#4a5568;'>Producto: <strong>{H(productName)}</strong></p>
          <p style='margin:6px 0;color:#4a5568;'>Cantidad: <strong>{quantityRequested}</strong></p>
          <p style='margin:6px 0;color:#4a5568;'>Total: <strong>{Cop(total)}</strong></p>
          <p style='margin:6px 0;color:#718096; font-size:13px;'>Expirado: {H(Local(expiredAtUtc))}</p>
        </div>";
            await SendAsync(emailReceptor, subject, Layout(body));
        }
    }
}
