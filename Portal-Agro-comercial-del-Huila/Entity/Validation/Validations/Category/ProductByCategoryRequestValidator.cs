using Entity.DTOs.Category;
using FluentValidation;

namespace Entity.Validation.Validations.Category
{
    public class ProductByCategoryRequestValidator : AbstractValidator<ProductByCategoryRequestDto>
    {
        public ProductByCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryIds)
               .NotEmpty().WithMessage("Debe seleccionar al menos una categoría.");
        }
    }
}
