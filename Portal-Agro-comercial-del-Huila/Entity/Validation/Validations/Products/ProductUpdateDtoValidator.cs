using Entity.DTOs.Products.Update;
using FluentValidation;
using Microsoft.AspNetCore.Http;

public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
{
    public ProductUpdateDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        // Id (asumiendo que viene de BaseDto)
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id inválido.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nombre obligatorio.")
            .Must(NotWhiteSpace).WithMessage("Nombre inválido.")
            .Length(2, 100).WithMessage("Nombre 2–100 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descripción obligatoria.")
            .Must(NotWhiteSpace).WithMessage("Descripción inválida.")
            .Length(5, 500).WithMessage("Descripción 5–500 caracteres.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Precio > 0.")
            .LessThanOrEqualTo(1_000_000).WithMessage("Precio demasiado alto.");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Unidad obligatoria.")
            .Must(NotWhiteSpace).WithMessage("Unidad inválida.")
            .MaximumLength(20).WithMessage("Unidad máx. 20.");

        RuleFor(x => x.Production)
            .NotEmpty().WithMessage("Producción obligatoria.")
            .Must(NotWhiteSpace).WithMessage("Producción inválida.")
            .MaximumLength(50).WithMessage("Producción máx. 50.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock no puede ser negativo.")
            .LessThanOrEqualTo(100_000).WithMessage("Stock demasiado alto.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Categoría inválida.");

        //RuleFor(x => x.FarmId)
        //    .GreaterThan(0).WithMessage("Finca inválida.");

        // Images: opcional, pero si vienen, limitar cantidad/tamaño/tipo
        RuleFor(x => x.Images)
            .Must(list => list == null || list.Count <= 5)
            .WithMessage("Máximo 5 imágenes nuevas.");

        //RuleForEach(x => x.Images).ChildRules(img =>
        //{
        //    img.RuleFor(i => i.Length)
        //       .LessThanOrEqualTo(2 * 1024 * 1024).WithMessage("Cada imagen ≤ 2MB.");

        //    img.RuleFor(i => i.ContentType)
        //       .Must(t => t == "image/jpeg" || t == "image/png")
        //       .WithMessage("Solo JPEG o PNG.");
        //});

        // ImagesToDelete: opcional, pero si vienen, que no tengan vacíos
        //RuleForEach(x => x.ImagesToDelete)
        //    .NotEmpty().WithMessage("Id de imagen a eliminar inválido.");
    }

    private static bool NotWhiteSpace(string? s) => !string.IsNullOrWhiteSpace(s);
}
