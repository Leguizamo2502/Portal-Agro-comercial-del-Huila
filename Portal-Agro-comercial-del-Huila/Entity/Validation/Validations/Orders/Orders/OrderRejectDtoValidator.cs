// Entity.Validations/Orders/OrderRejectDtoValidator.cs
using Entity.DTOs.Order.Create;
using FluentValidation;
using System;

public class OrderRejectDtoValidator : AbstractValidator<OrderRejectDto>
{
    public OrderRejectDtoValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(300);

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
