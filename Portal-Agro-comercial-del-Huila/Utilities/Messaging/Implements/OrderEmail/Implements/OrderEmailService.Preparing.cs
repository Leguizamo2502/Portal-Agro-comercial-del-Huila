namespace Utilities.Messaging.Implements
{
    public partial class OrderEmailService
    {
        public async Task SendOrderPreparingToCustomer(
            string emailReceptor, int orderId, string productName, int quantityRequested,
            decimal total, DateTime preparingAtUtc)
        {
            var subject = $"Tu pedido #{orderId} está en preparación";
            var body = $@"
<p style='margin:0 0 16px;color:#4a5568;'>
  ¡Buenas noticias! Tu pedido pasó a <strong>preparación</strong>.
</p>
<div style='border:1px solid #e2e8f0;border-radius:8px;padding:16px;margin:16px 0;'>
  <p style='margin:6px 0;color:#4a5568;'>Pedido: <strong>#{orderId}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Producto: <strong>{H(productName)}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Cantidad: <strong>{quantityRequested}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Total: <strong>{Cop(total)}</strong></p>
  <p style='margin:6px 0;color:#718096;font-size:13px;'>Desde: {H(Local(preparingAtUtc))}</p>
</div>";
            await SendAsync(emailReceptor, subject, Layout(body));
        }

        public async Task SendOrderDispatchedToCustomer(
            string emailReceptor, int orderId, string productName, int quantityRequested,
            decimal total, DateTime dispatchedAtUtc)
        {
            var subject = $"Tu pedido #{orderId} fue despachado";
            var body = $@"
<p style='margin:0 0 16px;color:#4a5568;'>
  Tu pedido fue <strong>despachado</strong>. Te avisaremos al marcarse como entregado.
</p>
<div style='border:1px solid #e2e8f0;border-radius:8px;padding:16px;margin:16px 0;'>
  <p style='margin:6px 0;color:#4a5568;'>Pedido: <strong>#{orderId}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Producto: <strong>{H(productName)}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Cantidad: <strong>{quantityRequested}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Total: <strong>{Cop(total)}</strong></p>
  <p style='margin:6px 0;color:#718096;font-size:13px;'>Despachado: {H(Local(dispatchedAtUtc))}</p>
</div>";
            await SendAsync(emailReceptor, subject, Layout(body));
        }

        public async Task SendOrderDeliveredToCustomer(
            string emailReceptor, int orderId, string productName, int quantityRequested,
            decimal total, DateTime deliveredAtUtc)
        {
            var subject = $"Pedido #{orderId} entregado — confírmalo";
            var body = $@"
<p style='margin:0 0 16px;color:#4a5568;'>
  El productor marcó tu pedido como <strong>entregado</strong>. Por favor confirma la recepción o reporta un problema.
</p>
<div style='border:1px solid #e2e8f0;border-radius:8px;padding:16px;margin:16px 0;'>
  <p style='margin:6px 0;color:#4a5568;'>Pedido: <strong>#{orderId}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Producto: <strong>{H(productName)}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Cantidad: <strong>{quantityRequested}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Total: <strong>{Cop(total)}</strong></p>
  <p style='margin:6px 0;color:#718096;font-size:13px;'>Entregado: {H(Local(deliveredAtUtc))}</p>
</div>
<p style='margin:0;color:#4a5568;'>
  Ingresa a tu cuenta para confirmar o reportar un problema.
</p>";
            await SendAsync(emailReceptor, subject, Layout(body));
        }

        public async Task SendOrderCancelledByUserToProducer(
            string emailReceptor, int orderId, string productName, int quantityRequested,
            DateTime cancelledAtUtc)
        {
            var subject = $"Pedido #{orderId} cancelado por el cliente";
            var body = $@"
<p style='margin:0 0 16px;color:#4a5568;'>
  El cliente canceló el pedido antes de tu decisión.
</p>
<div style='border:1px solid #e2e8f0;border-radius:8px;padding:16px;margin:16px 0;'>
  <p style='margin:6px 0;color:#4a5568;'>Pedido: <strong>#{orderId}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Producto: <strong>{H(productName)}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Cantidad: <strong>{quantityRequested}</strong></p>
  <p style='margin:6px 0;color:#718096;font-size:13px;'>Cancelado: {H(Local(cancelledAtUtc))}</p>
</div>";
            await SendAsync(emailReceptor, subject, Layout(body));
        }

        public async Task SendOrderCompletedToCustomer(
            string emailReceptor,
            int orderId,
            string productName,
            int quantityRequested,
            decimal total,
            DateTime completedAtUtc,
            bool autoCompleted = false)
        {
            var subject = autoCompleted
                ? $"Pedido #{orderId} completado automáticamente"
                : $"Tu pedido #{orderId} fue completado";

            var intro = autoCompleted
                ? "Tu pedido fue completado automáticamente al cumplirse el plazo de confirmación. Si algo no está bien, contáctanos."
                : "Tu pedido quedó marcado como completado. ¡Gracias por tu compra!";

            var body = $@"
<h2 style='color:#2d3748; margin:0 0 8px;'>Hola</h2>
<p style='margin:0 0 16px; color:#4a5568;'>{H(intro)}</p>

<div style='border:1px solid #e2e8f0; border-radius:8px; padding:16px; margin:16px 0;'>
  <p style='margin:6px 0;color:#4a5568;'>Pedido: <strong>#{orderId}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Producto: <strong>{H(productName)}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Cantidad: <strong>{quantityRequested}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Total: <strong>{Cop(total)}</strong></p>
  <p style='margin:6px 0;color:#718096; font-size:13px;'>Completado: {H(Local(completedAtUtc))}</p>
</div>

<p style='margin:16px 0; color:#4a5568; font-size:14px;'>
  ¿Necesitas ayuda? Responde a este correo y con gusto te asistimos.
</p>";

            await SendAsync(emailReceptor, subject, Layout(body));
        }
    }
}
