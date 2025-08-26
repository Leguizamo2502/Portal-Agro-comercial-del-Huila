using Entity.DTOs.Security.Create.RolFormPermission;
using FluentValidation;

namespace Entity.Validation.Validations.Security.RolFormPermission
{
    public class RolFormPermissionRegisterDtoValidator : AbstractValidator<RolFormPermissionRegisterDto>
    {
        public RolFormPermissionRegisterDtoValidator()
        {

            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.RolId)
                .GreaterThan(0).WithMessage("Debe seleccionar un rol válido.");

            RuleFor(x => x.PermissionId)
                .GreaterThan(0).WithMessage("Debe seleccionar un permiso válido.");

            RuleFor(x => x.FormId)
                .GreaterThan(0).WithMessage("Debe seleccionar un formulario válido.");

        }
    }
}
