using Entity.DTOs.Security.Create.FormModule;
using FluentValidation;

public class FormModuleRegisterDtoValidator : AbstractValidator<FormModuleRegisterDto>
{
    public FormModuleRegisterDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FormId)
            .GreaterThan(0).WithMessage("Debe seleccionar un formulario válido.");

        RuleFor(x => x.ModuleId)
            .GreaterThan(0).WithMessage("Debe seleccionar un módulo válido.");
    }
}
