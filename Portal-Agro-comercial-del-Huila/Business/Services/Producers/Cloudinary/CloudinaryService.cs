using Business.Interfaces.Implements.Producers.Cloudinary;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Utilities.Exceptions;

namespace Business.Services.Producers.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        // Configuración personalizada
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long _maxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
        public CloudinaryService(CloudinaryDotNet.Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }
        /*
         
         Farms
         
         
         
         */
        public async Task<ImageUploadResult> UploadFarmImagesAsync(IFormFile file, int farmid)
        {
            ValidateImage(file);



            //var safeName = name.Replace(" ", "_").ToLowerInvariant();
            var fileName = $"farm_{farmid}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var folder = $"farms/{farmid}";

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                PublicId = $"img_{Guid.NewGuid()}",
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
                    .Width(1200)
                    .Crop("limit")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null || string.IsNullOrWhiteSpace(result.SecureUrl?.AbsoluteUri))
            {
                throw new BusinessException($"Error al subir imagen: {result.Error?.Message ?? "Respuesta vacía o inválida"}");
            }

            return result;
        }




        /*
         
         Products
         */

        public async Task<ImageUploadResult> UploadProductImagesAsync(IFormFile file, int productid)
        {


            ValidateImage(file);

 

                //var safeName = name.Replace(" ", "_").ToLowerInvariant();
                var fileName = $"product_{productid}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var folder = $"products/{productid}";

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                PublicId = $"img_{Guid.NewGuid()}",
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
                    .Width(1200)
                    .Crop("limit")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null || string.IsNullOrWhiteSpace(result.SecureUrl?.AbsoluteUri))
            {
                throw new BusinessException($"Error al subir imagen: {result.Error?.Message ?? "Respuesta vacía o inválida"}");
            }

            return result;

        }

        public async Task<ImageUploadResult> UploadOrderPaymentImageAsync(IFormFile file, int orderId)
        {
            ValidateImage(file);

            var folder = $"orders/{orderId}";
            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                PublicId = $"payment_{orderId}_{Guid.NewGuid()}",
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
                    .Width(1600) // suficiente para comprobantes; ajusta si quieres
                    .Crop("limit")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null || string.IsNullOrWhiteSpace(result.SecureUrl?.AbsoluteUri))
                throw new BusinessException($"Error al subir imagen: {result.Error?.Message ?? "Respuesta inválida"}");

            return result;
        }

        public async Task<ImageUploadResult> UploadBytesAsync(
             byte[] data,
             string folder,
             string publicId,                 // sin extensión
             string fileNameWithExtension,    // ej: "qr_ABC123.png"
             string contentType,              // no lo usa Cloudinary; se infiere por extensión
             bool overwrite = true)
        {
            if (data is null || data.Length == 0)
                throw new BusinessException("No hay datos para subir.");

            using var ms = new MemoryStream(data);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileNameWithExtension, ms),
                Folder = folder?.Trim('/'),          // p.ej. "producers/42"
                PublicId = publicId,                  // p.ej. "qr_png"
                Overwrite = overwrite,                // permite regenerar
                UseFilename = false,
                UniqueFilename = false,
                Type = "upload",
                Invalidate = true                     // opcional: fuerza invalidación CDN
                                                      // ResourceType: NO asignar; es readonly y ya es "image"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null || string.IsNullOrWhiteSpace(result.SecureUrl?.AbsoluteUri))
                throw new BusinessException($"Error al subir QR: {result.Error?.Message ?? "Respuesta inválida"}");

            return result;
        }





        //Helpers

        public async Task DeleteAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                throw new BusinessException("PublicId no puede estar vacío.");

            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (result.Result != "ok" && result.Result != "not_found")
                throw new BusinessException($"Error al eliminar imagen: {result.Error?.Message ?? result.Result}");
        }
        public string ExtractPublicId(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var path = uri.AbsolutePath; // /djj163sc9/image/upload/v1753390824/farms/4/img_<guid>.png

            // Encuentra la parte después de "/upload/"
            var uploadIndex = path.IndexOf("/upload/");
            if (uploadIndex == -1)
                throw new BusinessException("La URL no tiene el formato esperado de Cloudinary.");

            var startIndex = uploadIndex + "/upload/".Length;
            var publicIdWithVersion = path.Substring(startIndex); // v1753390824/farms/4/img_<guid>.png

            // Quita el prefijo de versión (v...) si lo hay
            var segments = publicIdWithVersion.Split('/', 2);
            if (segments.Length < 2)
                throw new BusinessException("La URL no contiene un publicId válido.");

            var publicIdWithExtension = segments[1]; // farms/4/img_<guid>.png

            // Quitar extensión
            var publicId = Path.Combine(Path.GetDirectoryName(publicIdWithExtension) ?? "",
                                        Path.GetFileNameWithoutExtension(publicIdWithExtension))
                                        .Replace("\\", "/"); // Garantizar formato UNIX

            return publicId;
        }


        private void ValidateImage(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                throw new BusinessException("El archivo de imagen está vacío.");

            if (file.Length > _maxFileSizeInBytes)
                throw new BusinessException("El tamaño máximo permitido es de 5 MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new BusinessException($"Extensión de archivo no permitida. Extensiones válidas: {string.Join(", ", _allowedExtensions)}");
        }

        
    }
}
