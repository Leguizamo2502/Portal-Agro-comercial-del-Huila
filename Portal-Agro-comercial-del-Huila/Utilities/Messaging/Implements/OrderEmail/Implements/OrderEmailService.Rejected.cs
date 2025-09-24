using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Messaging.Implements
{
    public partial class OrderEmailService
    {
        public async Task SendOrderRejectedToCustomer(
            string emailReceptor,
            int orderId,
            string productName,
            int quantityRequested,
            string reason,
            DateTime decisionAtUtc)
        {
            var subject = $"Tu pedido #{orderId} fue rechazado";
            var body = $@"
<h2 style='color:#2d3748; margin:0 0 8px;'>Hola</h2>
<p style='margin:0 0 16px; color:#4a5568;'>Tu pedido fue rechazado.</p>

<div style='border:1px solid #e2e8f0; border-radius:8px; padding:16px; margin:16px 0;'>
  <p style='margin:6px 0;color:#4a5568;'>Pedido: <strong>#{orderId}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Producto: <strong>{H(productName)}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Cantidad: <strong>{quantityRequested}</strong></p>
  <p style='margin:6px 0;color:#4a5568;'>Motivo: <strong>{H(reason)}</strong></p>
  <p style='margin:6px 0;color:#718096; font-size:13px;'>Fecha: {H(Local(decisionAtUtc))}</p>
</div>";

            await SendAsync(emailReceptor, subject, Layout(body));
        }
    }
}
