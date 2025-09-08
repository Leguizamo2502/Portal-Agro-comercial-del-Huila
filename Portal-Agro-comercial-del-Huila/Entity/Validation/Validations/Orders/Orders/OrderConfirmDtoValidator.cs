// Entity.Validations/Orders/OrderConfirmDtoValidator.cs
using Entity.DTOs.Order.Create;
using FluentValidation;
using System;

public class OrderConfirmDtoValidator : AbstractValidator<OrderConfirmDto>
{
    public OrderConfirmDtoValidator()
    {
        RuleFor(x => x.Answer)
            .NotEmpty()
            .Must(a => a.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                       a.Equals("no", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Answer debe ser 'Yes' o 'No'.");

        RuleFor(x => x.RowVersion)
            .NotEmpty()
            .Must(IsBase64).WithMessage("RowVersion inválido.");
    }

    private static bool IsBase64(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        try { Convert.FromBase64String(value); return true; }
        catch { return false; }
    }
}
