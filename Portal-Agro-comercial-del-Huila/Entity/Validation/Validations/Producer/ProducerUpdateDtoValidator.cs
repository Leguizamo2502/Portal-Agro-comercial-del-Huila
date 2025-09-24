using Entity.DTOs.Producer.Producer.Update;
using FluentValidation;

namespace Entity.Validation.Validations.Producer
{
    public class ProducerUpdateDtoValidator : AbstractValidator<ProducerUpdateDto>
    {
        public ProducerUpdateDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .Length(5, 500).WithMessage("La descripción debe tener entre 5 y 500 caracteres.");
        }
    }
}
