using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Order.Reviews;
using FluentValidation;

namespace Entity.Validation.Validations.Orders.Review
{
    public class ReviewCreateDtoValidator : AbstractValidator<ReviewCreateDto>
    {
        public ReviewCreateDtoValidator()
        {
            RuleFor(x => x.ProductId)
           .GreaterThan(0).WithMessage("El ProductId debe ser mayor que 0.");

            RuleFor(x => x.Rating)
                .InclusiveBetween((byte)1, (byte)5)
                .WithMessage("El Rating debe estar entre 1 y 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("El comentario es obligatorio.")
                .MaximumLength(500).WithMessage("El comentario no puede superar los 500 caracteres.");
        }
    }
}
