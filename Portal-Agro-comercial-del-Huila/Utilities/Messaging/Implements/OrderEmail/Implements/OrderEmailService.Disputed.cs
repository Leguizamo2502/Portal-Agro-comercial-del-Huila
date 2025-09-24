namespace Utilities.Messaging.Implements
{
    public partial class OrderEmailService
    {
        public async Task SendOrderDisputedToProducer(
            string emailReceptor,
            int orderId,
            string productName,
            int quantityRequested,
            decimal total,
            DateTime disputedAtUtc)
        {
            var subject = $"Pedido #{orderId} en disputa";
            var body = $@"
<h2 style='color:#2d3748; margin:0 0 8px;'>Hola</h2>
<p style='margin:0 0 16px; color:#4a5568;'>El cliente reportó que NO recibió el pedido.</p>

<div style='border:1px solid #e2e8f0; border-radius:8px; padding:16px; margin:16px 0;'>
  <p style='margin:6px 0;color:#4a5568;'>Pedido: <strong>#{orderId}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Producto: <strong>{H(productName)}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Cantidad: <strong>{quantityRequested}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Total: <strong>{Cop(total)}</strong></p>
  <p style='margin:6px 0;color:#718096; font-size:13px;'>Reportado: {H(Local(disputedAtUtc))}</p>
</div>";

            await SendAsync(emailReceptor, subject, Layout(body));
        }
    }
}
