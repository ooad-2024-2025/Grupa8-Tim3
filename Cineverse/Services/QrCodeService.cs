namespace Cineverse.Services
{
    using QRCoder;

    public class QrCodeService
    {
        public byte[] GenerateQrCode(string content)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var pngQrCode = new PngByteQRCode(qrCodeData);
            return pngQrCode.GetGraphic(20);
        }
    }
}
