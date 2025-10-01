using Entity.DTOs.Products.Update;
using FluentValidation;

namespace Entity.Validation.Validations.Products
{
    public class UpdateStockDtoValidator : AbstractValidator<UpdateStockDto>
    {
        public UpdateStockDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Id de producto inválido.");
            RuleFor(x => x.NewStock)
                .GreaterThanOrEqualTo(0).WithMessage("El nuevo stock no puede ser negativo.")
                .LessThanOrEqualTo(100_000).WithMessage("El nuevo stock es demasiado alto.");
        }

    }
}
