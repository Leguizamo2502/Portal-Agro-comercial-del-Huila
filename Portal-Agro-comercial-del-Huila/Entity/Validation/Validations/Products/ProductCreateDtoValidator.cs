using Entity.DTOs.Products.Create;
using FluentValidation;
using Microsoft.AspNetCore.Http;

public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        // Nombre
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
            .Must(NotWhiteSpace).WithMessage("El nombre no puede estar en blanco.")
            .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres.");

        // Descripción
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .Must(NotWhiteSpace).WithMessage("La descripción no puede estar en blanco.")
            .Length(5, 500).WithMessage("La descripción debe tener entre 5 y 500 caracteres.");

        // Precio
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.")
            .LessThanOrEqualTo(100000000).WithMessage("El precio no puede superar 100,000,000.");

        // Unidad (ej: kg, litro, unidad, etc.)
        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("La unidad es obligatoria.")
            .Must(NotWhiteSpace).WithMessage("La unidad no puede estar en blanco.")
            .MaximumLength(20).WithMessage("La unidad no debe superar 20 caracteres.");

        // Producción (ej: orgánico, convencional, etc.)
        RuleFor(x => x.Production)
            .NotEmpty().WithMessage("El tipo de producción es obligatorio.")
            .Must(NotWhiteSpace).WithMessage("El tipo de producción no puede estar en blanco.")
            .MaximumLength(150).WithMessage("El tipo de producción no debe superar 150 caracteres.");

        // Stock
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.")
            .LessThanOrEqualTo(100000).WithMessage("El stock no puede superar 100,000 unidades.");

        // Estado (bool, ya está implícito, no requiere validación extra a menos que definas política)

        // Categoría
        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Debe seleccionar una categoría válida.");

        // Granja
        RuleFor(x => x.FarmId)
            .GreaterThan(0).WithMessage("Debe seleccionar una finca válida.");

        // Imágenes
        RuleFor(x => x.Images)
            .NotNull().WithMessage("Debe adjuntar al menos una imagen.")
            .Must(i => i != null && i.Any()).WithMessage("Debe adjuntar al menos una imagen.")
            .Must(i => i.Count <= 5).WithMessage("No puede adjuntar más de 5 imágenes.");

        //RuleForEach(x => x.Images).ChildRules(images =>
        //{
        //    images.RuleFor(img => img.Length)
        //        .LessThanOrEqualTo(2 * 1024 * 1024) // 2 MB
        //        .WithMessage("Cada imagen no debe superar los 2 MB.");

        //    images.RuleFor(img => img.ContentType)
        //        .Must(type => type == "image/jpeg" || type == "image/png")
        //        .WithMessage("Solo se permiten imágenes en formato JPEG o PNG.");
        //});
    }

    private static bool NotWhiteSpace(string? s) =>
        !string.IsNullOrWhiteSpace(s);
}
