using System.Security.Cryptography;
using System.Text;

namespace Utilities.Custom.Code
{
    public static class CodeGenerator
    {
        // Conjunto completo de caracteres (62)
        private const string FullAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Variante "segura" (sin caracteres ambiguos: I, l, 1, O, 0)
        private const string SafeAlphabet = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";

        /// <summary>
        /// Genera un código aleatorio opaco.
        /// </summary>
        /// <param name="length">Longitud del código (ej. 10)</param>
        /// <param name="safe">Si es true, excluye caracteres ambiguos</param>
        /// <returns>Código aleatorio único</returns>
        public static string Generate(int length = 10, bool safe = true)
        {
            if (length <= 0)
                throw new ArgumentException("La longitud debe ser mayor a cero.", nameof(length));

            var alphabet = safe ? SafeAlphabet : FullAlphabet;
            var result = new StringBuilder(length);

            using var rng = RandomNumberGenerator.Create();
            var buffer = new byte[sizeof(uint)];

            for (int i = 0; i < length; i++)
            {
                rng.GetBytes(buffer);
                uint num = BitConverter.ToUInt32(buffer, 0);
                result.Append(alphabet[(int)(num % (uint)alphabet.Length)]);
            }

            return result.ToString();
        }
    }
}
