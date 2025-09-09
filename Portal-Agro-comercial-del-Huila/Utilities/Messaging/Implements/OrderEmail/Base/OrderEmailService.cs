using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using Utilities.Messaging.Interfaces;

namespace Utilities.Messaging.Implements
{
    public partial class OrderEmailService : IOrderEmailService
    {
        private readonly IConfiguration _config;

        public OrderEmailService(IConfiguration config)
        {
            _config = config;
        }

        // Comentario: lee y valida configuración SMTP
        private (string From, string? Password, string Host, int Port) GetSmtpSettings()
        {
            var from = _config["CONFIG_EMAIL:EMAIL"];
            var pwd = _config["CONFIG_EMAIL:PASSWORD"];
            var host = _config["CONFIG_EMAIL:HOST"];
            var port = _config["CONFIG_EMAIL:PORT"];

            if (string.IsNullOrWhiteSpace(from))
                throw new InvalidOperationException("SMTP remitente no configurado (CONFIG_EMAIL:EMAIL).");
            if (string.IsNullOrWhiteSpace(host))
                throw new InvalidOperationException("SMTP host no configurado (CONFIG_EMAIL:HOST).");
            if (!int.TryParse(port, out var p))
                throw new InvalidOperationException("SMTP port inválido (CONFIG_EMAIL:PORT).");

            return (from, pwd, host, p);
        }

        // Comentario: envío genérico centralizado
        private async Task SendAsync(string to, string subject, string htmlBody)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("emailReceptor es obligatorio.", nameof(to));

            var (from, pwd, host, port) = GetSmtpSettings();

            using var smtp = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, pwd)
            };

            using var message = new MailMessage(from, to, subject, htmlBody) { IsBodyHtml = true };
            await smtp.SendMailAsync(message);
        }

        // ================= Helpers de HTML/formatos =================

        // Comentario: codifica HTML defensivo
        private static string H(string? s) => WebUtility.HtmlEncode(s ?? string.Empty);

        // Comentario: formato COP sin decimales (es-CO)
        private static string Cop(decimal value) => value.ToString("C0", new CultureInfo("es-CO"));

        // Comentario: fecha local legible
        private static string Local(DateTime utc) => utc.ToLocalTime().ToString("dd/MM/yyyy HH:mm");

        // Comentario: layout común para todos los correos
        private static string Layout(string innerHtml) => $@"
            <!DOCTYPE html>
            <html lang='es'>
            <head><meta charset='UTF-8'><title>Notificación</title></head>
            <body style='font-family: Arial, sans-serif; background:#f4f4f4; padding:40px;'>
              <div style='max-width: 640px; margin:auto; background:#fff; padding:28px; border-radius:10px; box-shadow:0 5px 15px rgba(0,0,0,0.08);'>
                {innerHtml}
                <p style='font-size:12px; color:#a0aec0; margin-top:24px;'>Portal Agro-Comercial del Huila © 2025</p>
              </div>
            </body>
            </html>";
    }

}

