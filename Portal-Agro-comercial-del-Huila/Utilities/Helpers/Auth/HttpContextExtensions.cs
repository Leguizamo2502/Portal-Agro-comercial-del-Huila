using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Utilities.Helpers.Auth
{
    public static class HttpContextExtensions
    {
        public static int GetUserId(this HttpContext http)
        {
            var claim = http.User.FindFirst(ClaimTypes.NameIdentifier)
                        ?? http.User.FindFirst("sub");
            if (claim is null)
                throw new UnauthorizedAccessException("No se encontró el userId en el token.");
            if (!int.TryParse(claim.Value, out var userId))
                throw new UnauthorizedAccessException("El userId del token no es válido, intenta de nuevo.");
            return userId;
        }

        // Nueva: devuelve null si no hay usuario o no parsea.
        public static int? TryGetUserId(this HttpContext http)
        {
            var claim = http.User.FindFirst(ClaimTypes.NameIdentifier)
                        ?? http.User.FindFirst("sub");
            if (claim is null) return null;
            return int.TryParse(claim.Value, out var userId) ? userId : (int?)null;
        }
    }
}
