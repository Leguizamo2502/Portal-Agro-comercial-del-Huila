// Entity/DTOs/Validations/Orders/OrderRejectDtoValidator.cs
using Entity.DTOs.Order.Create;
using FluentValidation;

namespace Entity.DTOs.Validations.Orders
{
    public class OrderRejectDtoValidator : AbstractValidator<OrderRejectDto>
    {
        public OrderRejectDtoValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("El motivo es obligatorio.")
                .MinimumLength(10).WithMessage("El motivo debe tener al menos 10 caracteres.")
                .MaximumLength(500);
        }
    }
}
