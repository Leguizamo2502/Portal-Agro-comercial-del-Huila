using Entity.Domain.Enums;
using Utilities.Exceptions;

namespace Utilities.Helpers.Business
{
    public static class Urls
    {
        public static string NormalizeUrl(SocialNetwork net, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new BusinessException("La URL de la red social es obligatoria.");

            input = input.Trim();

            if (Uri.TryCreate(input, UriKind.Absolute, out var uri) &&
                (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp))
                return uri.ToString();

            return net switch
            {
                SocialNetwork.Instagram => $"https://instagram.com/{input.TrimStart('@')}",
                SocialNetwork.Facebook => $"https://facebook.com/{input}",
                SocialNetwork.Whatsapp => BuildWhatsAppUrl(input),
                SocialNetwork.X => $"https://x.com/{input.TrimStart('@')}",
                SocialNetwork.Website => $"https://{input}",
                _ => input.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? input : $"https://{input}"
            };
        }

        public static string BuildWhatsAppUrl(string phone)
        {
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(digits) || digits.Length < 10)
                throw new BusinessException("Número de WhatsApp inválido.");
            return $"https://wa.me/{digits}";
        }

    }
}
