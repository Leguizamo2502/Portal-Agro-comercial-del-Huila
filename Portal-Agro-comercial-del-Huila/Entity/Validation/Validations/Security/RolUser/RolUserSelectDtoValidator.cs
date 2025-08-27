using Entity.DTOs.Security.Create.RolUser;
using Entity.DTOs.Security.Selects.RolUser;
using FluentValidation;


namespace Entity.Validation.Validations.Security.RolUser
{
    public class RolUserSelectDtoValidator : AbstractValidator<RolUserSelectDto>
    {
        public RolUserSelectDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.RolId)
                .GreaterThan(0).WithMessage("Debe seleccionar un rol válido.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Debe seleccionar un usuario válido.");
        }
    }
}
