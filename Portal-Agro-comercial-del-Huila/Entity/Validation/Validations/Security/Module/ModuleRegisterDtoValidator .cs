using Entity.DTOs.Security.Create.NewFolder;
using FluentValidation;

public class ModuleRegisterDtoValidator : AbstractValidator<ModuleRegisterDto>
{
    public ModuleRegisterDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NameRules();
        RuleFor(x => x.Description).DescriptionRules();
    }
}
