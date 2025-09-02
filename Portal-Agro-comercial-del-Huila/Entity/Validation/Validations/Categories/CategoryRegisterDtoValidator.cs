using Entity.DTOs.Producer.Categories;
using FluentValidation;

public class CategoryRegisterDtoValidator : AbstractValidator<CategoryRegisterDto>
{
    public CategoryRegisterDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            //.Must(s => !string.IsNullOrWhiteSpace(s))
              //  .WithMessage("El nombre no puede estar en blanco.")
            // Validar longitud sobre el valor recortado
            .Must(s =>
            {
                var len = (s ?? string.Empty).Trim().Length;
                return len >= 5 && len <= 100;
            })
                .WithMessage("El nombre debe tener entre 5 y 100 caracteres.");

        RuleFor(x => x.ParentCategoryId)
            .Must(id => id is null || id > 0)
            .WithMessage("La categoría padre debe ser un identificador válido (> 0) o estar vacía.");
    }
}
