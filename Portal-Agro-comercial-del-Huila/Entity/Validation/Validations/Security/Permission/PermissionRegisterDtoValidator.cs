using Entity.DTOs.Security.Create.Permissions;
using FluentValidation;

public class PermissionRegisterDtoValidator : AbstractValidator<PermissionRegisterDto>
{
    public PermissionRegisterDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NameRules();
        RuleFor(x => x.Description).DescriptionRules();
    }
}
