// Entity/DTOs/Validations/Orders/OrderAcceptDtoValidator.cs
using Entity.DTOs.Order.Create;
using FluentValidation;

namespace Entity.DTOs.Validations.Orders
{
    public class OrderAcceptDtoValidator : AbstractValidator<OrderAcceptDto>
    {
        public OrderAcceptDtoValidator()
        {
            RuleFor(x => x.DeliveryFee)
                .GreaterThanOrEqualTo(0m).WithMessage("El envío no puede ser negativo.")
                .LessThanOrEqualTo(200_000m).WithMessage("El envío excede el límite permitido.");

            RuleFor(x => x.DeliveryNotes)
                .MaximumLength(500);
        }
    }
}
