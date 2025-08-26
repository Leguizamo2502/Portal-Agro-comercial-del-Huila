using Entity.DTOs.Security.Create.Rols;
using FluentValidation;

public class RolRegisterDtoValidator : AbstractValidator<RolRegisterDto>
{
    public RolRegisterDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NameRules();
        RuleFor(x => x.Description).DescriptionRules();
    }
}
