using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Messaging.Implements
{
    public partial class OrderEmailService
    {
        public async Task SendOrderCreatedEmail(
            string emailReceptor,
            int orderId,
            string productName,
            int quantityRequested,
            decimal subtotal,
            decimal total,
            DateTime createdAtUtc,
            string? personName = null,
            string? counterpartName = null,
            bool isProducer = false)
        {
            var subject = isProducer
                ? $"Nuevo pedido #{orderId} pendiente de revisión"
                : $"Hemos recibido tu pedido #{orderId}";
            var greeting = string.IsNullOrWhiteSpace(personName) ? "Hola" : $"Hola, {H(personName)}";
            var subtitle = isProducer
                ? "Tienes un nuevo pedido pendiente de revisión."
                : "Recibimos tu pedido y estamos esperando la revisión del productor.";
            var counterpart = string.IsNullOrWhiteSpace(counterpartName)
                ? ""
                : $"<p style='margin:6px 0;color:#4a5568;'>Contraparte: <strong>{H(counterpartName)}</strong></p>";

            var body = $@"
<h2 style='color:#2d3748; margin:0 0 8px;'>{greeting}</h2>
<p style='margin:0 0 16px; color:#4a5568;'>{subtitle}</p>

<div style='border:1px solid #e2e8f0; border-radius:8px; padding:16px; margin:16px 0;'>
  <p style='margin:6px 0;color:#4a5568;'>Pedido: <strong>#{orderId}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Producto: <strong>{H(productName)}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Cantidad: <strong>{quantityRequested}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Subtotal: <strong>{Cop(subtotal)}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Total: <strong>{Cop(total)}</strong></p>
  <p style='margin:6px 0;color:#718096; font-size:13px;'>Creado: {H(Local(createdAtUtc))}</p>
  {counterpart}
</div>

<p style='margin-top:18px; color:#718096; font-size:13px;'>Si no esperabas este correo, puedes ignorarlo.</p>";

            await SendAsync(emailReceptor, subject, Layout(body));
        }
    }
}
