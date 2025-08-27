using Entity.DTOs.Security.Create.NewFolder;
using Entity.DTOs.Security.Selects.Module;
using FluentValidation;


public class ModuleSelectDtoValidator : AbstractValidator<ModuleSelectDto>
{
    public ModuleSelectDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NameRules();
        RuleFor(x => x.Description).DescriptionRules();
    }
}
