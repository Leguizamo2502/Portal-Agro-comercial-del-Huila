using Entity.DTOs.Products.Select;
using FluentValidation;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class ProductImageSelectDtoValidator : AbstractValidator<ProductImageSelectDto>
{
    // Extensiones permitidas para el nombre del archivo.
    private static readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

    public ProductImageSelectDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        // Id (de BaseDto)
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id de imagen inválido.");

        // URL de la imagen: obligatoria, no whitespace, URL absoluta http/https, longitud razonable
        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("La URL de la imagen es obligatoria.")
            .Must(NotWhiteSpace).WithMessage("La URL de la imagen es inválida.")
            .Must(BeValidHttpUrl).WithMessage("La URL de la imagen debe ser una URL http/https válida.")
            .MaximumLength(500).WithMessage("La URL de la imagen no debe superar 500 caracteres.");

        // FileName: obligatoria, sin whitespace, extensión válida, longitud razonable
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("El nombre del archivo es obligatorio.")
            .Must(NotWhiteSpace).WithMessage("El nombre del archivo es inválido.")
            .MaximumLength(200).WithMessage("El nombre del archivo no debe superar 200 caracteres.")
            .Must(HaveAllowedExtension).WithMessage($"El archivo debe tener una de las extensiones permitidas: {string.Join(", ", AllowedExtensions)}.");

        // PublicId: obligatorio, sin whitespace, caracteres seguros, longitud razonable
        RuleFor(x => x.PublicId)
            .NotEmpty().WithMessage("El identificador público es obligatorio.")
            .Must(NotWhiteSpace).WithMessage("El identificador público es inválido.")
            .MaximumLength(200).WithMessage("El identificador público no debe superar 200 caracteres.")
            .Matches(@"^[a-zA-Z0-9_\-\/\.]+$").WithMessage("El identificador público contiene caracteres no permitidos.");

        // ProductId: obligatorio (>0)
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Producto inválido para la imagen.");
    }

    private static bool NotWhiteSpace(string? s) => !string.IsNullOrWhiteSpace(s);

    private static bool BeValidHttpUrl(string url)
        => Uri.TryCreate(url, UriKind.Absolute, out var u)
           && (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps);

    private static bool HaveAllowedExtension(string fileName)
    {
        var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
        return !string.IsNullOrEmpty(ext) && Array.Exists(AllowedExtensions, e => e == ext);
    }
}
