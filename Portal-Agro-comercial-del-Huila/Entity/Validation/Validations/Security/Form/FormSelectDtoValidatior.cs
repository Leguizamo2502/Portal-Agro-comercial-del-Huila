using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.Rols;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class FormSelectDtoValidator : AbstractValidator<FormSelectDto>
    {
        public FormSelectDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            // URL
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("La URL es obligatoria.")
                .Must(s => Uri.TryCreate(s, UriKind.Absolute, out _))
                .WithMessage("La URL no tiene un formato válido.")
                .MaximumLength(200).WithMessage("La URL no debe superar 200 caracteres.");

            // Reglas comunes reutilizadas
            RuleFor(x => x.Name).NameRules();
            RuleFor(x => x.Description).DescriptionRules();

        }
    }
