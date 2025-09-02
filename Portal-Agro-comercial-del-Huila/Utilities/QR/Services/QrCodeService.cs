using Microsoft.Extensions.Options;
using QRCoder;
using Utilities.QR.Interfaces;
using Utilities.QR.Options;

namespace Utilities.QR.Services
{
    public sealed class QrCodeService : IQrCodeService
    {
        private readonly QrOptions _opt;
        public QrCodeService(IOptions<QrOptions> options) => _opt = options.Value;

        private static QRCodeGenerator.ECCLevel ToEcc(string ecc) => (ecc ?? "Q").ToUpperInvariant() switch
        {
            "L" => QRCodeGenerator.ECCLevel.L,
            "M" => QRCodeGenerator.ECCLevel.M,
            "Q" => QRCodeGenerator.ECCLevel.Q,
            "H" => QRCodeGenerator.ECCLevel.H,
            _ => QRCodeGenerator.ECCLevel.Q
        };

        public byte[] GeneratePng(string content)
        {
            using var gen = new QRCodeGenerator();
            using var data = gen.CreateQrCode(content, ToEcc(_opt.EccLevel));
            var png = new PngByteQRCode(data);
            return png.GetGraphic(_opt.PixelsPerModule, drawQuietZones: _opt.DrawQuietZones);
        }
    }
}
