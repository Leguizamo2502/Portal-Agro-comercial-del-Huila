namespace Utilities.Messaging.Implements
{
    public partial class OrderEmailService
    {
        public async Task SendOrderAcceptedAwaitingPaymentToCustomer(
            string emailReceptor,
            int orderId,
            string productName,
            int quantityRequested,
            decimal total,
            DateTime acceptedAtUtc,
            DateTime paymentDeadlineUtc)
        {
            var subject = $"Tu pedido #{orderId} fue aceptado – pendiente de pago";

            var body = $@"
<h2 style='color:#2d3748; margin:0 0 8px;'>Hola</h2>
<p style='margin:0 0 16px; color:#4a5568;'>
    Tu pedido fue aceptado por el productor. Para continuar, debes subir tu comprobante de pago antes de la fecha límite.
</p>

<div style='border:1px solid #e2e8f0; border-radius:8px; padding:16px; margin:16px 0;'>
  <p style='margin:6px 0;color:#4a5568;'>Pedido: <strong>#{orderId}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Producto: <strong>{H(productName)}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Cantidad: <strong>{quantityRequested}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Total: <strong>{Cop(total)}</strong></p>
  <p style='margin:6px 0;color:#718096; font-size:13px;'>Aceptado: {H(Local(acceptedAtUtc))}</p>
  <p style='margin:6px 0;color:#e53e3e; font-weight:bold;'>Fecha límite para subir comprobante: {H(Local(paymentDeadlineUtc))}</p>
</div>

<p style='margin:16px 0; color:#2d3748;'>
    Si no subes el comprobante a tiempo, tu pedido expirará automáticamente.
</p>";

            await SendAsync(emailReceptor, subject, Layout(body));
        }
    }
}
