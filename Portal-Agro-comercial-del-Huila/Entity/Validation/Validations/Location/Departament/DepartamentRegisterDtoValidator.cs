using Entity.DTOs.Location.Create;
using FluentValidation;

public class DepartmentRegisterDtoValidator : AbstractValidator<DepartmentRegisterDto>
{
    public DepartmentRegisterDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("El nombre no puede estar en blanco.")
            // Validar longitud sobre el valor recortado
            .Must(s =>
            {
                var len = (s ?? string.Empty).Trim().Length;
                return len >= 3 && len <= 100;
            })
                .WithMessage("El nombre debe tener entre 3 y 100 caracteres.");
    }
}
