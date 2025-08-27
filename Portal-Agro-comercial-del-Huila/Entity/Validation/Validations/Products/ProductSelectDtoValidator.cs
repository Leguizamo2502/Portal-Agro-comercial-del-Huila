using Entity.DTOs.Products.Select;
using FluentValidation;

public class ProductSelectDtoValidator : AbstractValidator<ProductSelectDto>
{
    public ProductSelectDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        // Id (de BaseDto)
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

        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Nombre de categoría obligatorio.")
            .Must(NotWhiteSpace).WithMessage("Nombre de categoría inválido.")
            .Length(2, 100).WithMessage("Nombre de categoría 2–100 caracteres.");

        RuleFor(x => x.FarmId)
            .GreaterThan(0).WithMessage("Finca inválida.");

        RuleFor(x => x.FarmName)
            .NotEmpty().WithMessage("Nombre de finca obligatorio.")
            .Must(NotWhiteSpace).WithMessage("Nombre de finca inválido.")
            .Length(2, 100).WithMessage("Nombre de finca 2–100 caracteres.");

        RuleFor(x => x.PersonName)
            .NotEmpty().WithMessage("Nombre de productor obligatorio.")
            .Must(NotWhiteSpace).WithMessage("Nombre de productor inválido.")
            .Length(2, 100).WithMessage("Nombre de productor 2–100 caracteres.");

        RuleFor(x => x.CityName)
            .NotEmpty().WithMessage("Ciudad obligatoria.")
            .Must(NotWhiteSpace).WithMessage("Ciudad inválida.")
            .Length(2, 100).WithMessage("Ciudad 2–100 caracteres.");

        RuleFor(x => x.DepartmentName)
            .NotEmpty().WithMessage("Departamento obligatorio.")
            .Must(NotWhiteSpace).WithMessage("Departamento inválido.")
            .Length(2, 100).WithMessage("Departamento 2–100 caracteres.");

        // Images: opcional en select; si vienen, limitar cantidad
        RuleFor(x => x.Images)
            .Must(list => list == null || list.Count <= 20)
            .WithMessage("Máximo 20 imágenes.");

        // IsFavorite y Status son booleanos -> sin reglas adicionales (si necesitas consistencia de negocio, se agregan aquí).
    }

    private static bool NotWhiteSpace(string? s) => !string.IsNullOrWhiteSpace(s);
}
