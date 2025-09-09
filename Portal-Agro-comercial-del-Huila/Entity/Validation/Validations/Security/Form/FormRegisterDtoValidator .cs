using Entity.DTOs.Security.Create.Rols;
using FluentValidation;

public class FormRegisterDtoValidator : AbstractValidator<FormRegisterDto>
{
    public FormRegisterDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        // URL
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("La URL es obligatoria.")
            .Must(s => Uri.TryCreate(s, UriKind.Absolute, out _))
            .WithMessage("La URL no tiene un formato válido.")
            .MaximumLength(200).WithMessage("La URL no debe superar 200 caracteres.")
            .Matches(@"^\S+$").WithMessage("La URL no debe contener espacios."); // <--- esta línea evita espacios

        // Reglas comunes reutilizadas
        RuleFor(x => x.Name).NameRules();
        RuleFor(x => x.Description).DescriptionRules();
    }
}
