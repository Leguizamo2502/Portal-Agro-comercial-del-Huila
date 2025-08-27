using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.Rols;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RolSelectDtoValidator : AbstractValidator<RolSelectDto>
{
    public RolSelectDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NameRules();
        RuleFor(x => x.Description).DescriptionRules();
    }
}
