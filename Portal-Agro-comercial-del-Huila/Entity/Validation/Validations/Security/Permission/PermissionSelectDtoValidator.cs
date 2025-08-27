
using Entity.DTOs.Security.Selects.Permissions;
using FluentValidation;


public class PermissionSelectDtoValidator : AbstractValidator<PermissionSelectDto>
{
    public PermissionSelectDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NameRules();
        RuleFor(x => x.Description).DescriptionRules();
    }
}
