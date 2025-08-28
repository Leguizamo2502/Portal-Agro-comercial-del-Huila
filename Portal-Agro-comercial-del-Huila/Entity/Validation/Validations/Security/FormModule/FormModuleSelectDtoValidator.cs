using Entity.DTOs.Security.Create.FormModule;
using Entity.DTOs.Security.Selects.FormModule;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FormModuleSelectDtoValidator : AbstractValidator<FormModuleSelectDto>
{
    public FormModuleSelectDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FormId)
            .GreaterThan(0).WithMessage("Debe seleccionar un formulario válido.");

        RuleFor(x => x.ModuleId)
            .GreaterThan(0).WithMessage("Debe seleccionar un módulo válido.");
    }
}
