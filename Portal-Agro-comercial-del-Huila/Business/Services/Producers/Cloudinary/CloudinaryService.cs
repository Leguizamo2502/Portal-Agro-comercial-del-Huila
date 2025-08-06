using Business.Interfaces.Implements.Producers.Cloudinary;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Products;
using Microsoft.AspNetCore.Http;
using Utilities.Exceptions;

namespace Business.Services.Producers.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;
        public CloudinaryService(CloudinaryDotNet.Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }
        public async Task<List<FarmImage>> UploadFarmImagesAsync(List<IFormFile> files, int farmid)
        {
            if (files == null || files.Count == 0)
                throw new BusinessException("Debe subir al menos una imagen.");

            if (files.Count > 5)
                throw new BusinessException("Solo se permiten hasta 5 imágenes por finca.");

            var images = new List<FarmImage>();

            foreach (var file in files)
            {
                if (file.Length <= 0)
                    continue;

                //var safeName = name.Replace(" ", "_").ToLowerInvariant();
                var fileName = $"farm_{farmid}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var folder = $"farms/{farmid}";

                var uploadParams = new ImageUploadParams
                {
                    PublicId = $"img_{Guid.NewGuid()}",
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = folder,
                    Transformation = new Transformation()
                    .Quality("auto")            // Ajusta calidad automáticamente
                    .FetchFormat("auto")        // Cambia el formato a WebP/AVIF si el cliente lo soporta
                    .Width(1200)                // Escala la imagen a 1200px de ancho (ajusta si quieres menos)
                    .Crop("limit")              // No agranda, solo reduce si es necesario
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new BusinessException($"Error al subir la imagen: {uploadResult.Error.Message}");

                images.Add(new FarmImage
                {
                    ImageUrl = uploadResult.SecureUrl.ToString()
                });
            }

            return images;
        }


        public async Task DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (result.Result != "ok")
                throw new BusinessException("No se pudo eliminar la imagen de Cloudinary.");
        }




        public async Task<List<ProductImage>> UploadProductImagesAsync(List<IFormFile> files, int productid)
        {
            if (files == null || files.Count == 0)
                throw new BusinessException("Debe subir al menos una imagen.");

            if (files.Count > 5)
                throw new BusinessException("Solo se permiten hasta 5 imágenes por producto.");

            var images = new List<ProductImage>();

            foreach (var file in files)
            {
                if (file.Length <= 0)
                    continue;

                //var safeName = name.Replace(" ", "_").ToLowerInvariant();
                var fileName = $"product_{productid}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var folder = $"products/{productid}";

                var uploadParams = new ImageUploadParams
                {
                    PublicId = $"img_{Guid.NewGuid()}",
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = folder,
                    Transformation = new Transformation()
                    .Quality("auto")            // Ajusta calidad automáticamente
                    .FetchFormat("auto")        // Cambia el formato a WebP/AVIF si el cliente lo soporta
                    .Width(1200)                // Escala la imagen a 1200px de ancho (ajusta si quieres menos)
                    .Crop("limit")              // No agranda, solo reduce si es necesario
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new BusinessException($"Error al subir la imagen: {uploadResult.Error.Message}");

                images.Add(new ProductImage
                {
                    ImageUrl = uploadResult.SecureUrl.ToString()
                });
            }

            return images;
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
    }
}
