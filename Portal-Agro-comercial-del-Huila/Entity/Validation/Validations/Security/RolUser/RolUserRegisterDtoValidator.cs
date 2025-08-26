using Entity.DTOs.Security.Create.RolUser;
using FluentValidation;

namespace Entity.Validation.Validations.Security.RolUser
{
    public class RolUserRegisterDtoValidator : AbstractValidator<RolUserRegisterDto>
    {
        public RolUserRegisterDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.RolId)
                .GreaterThan(0).WithMessage("Debe seleccionar un rol válido.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Debe seleccionar un usuario válido.");
        }
    }
}
