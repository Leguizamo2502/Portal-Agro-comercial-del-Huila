// Entity.Validations/Orders/OrderAcceptDtoValidator.cs
using Entity.DTOs.Order.Create;
using FluentValidation;
using System;

public class OrderAcceptDtoValidator : AbstractValidator<OrderAcceptDto>
{
    public OrderAcceptDtoValidator()
    {
        RuleFor(x => x.Notes)
            .MaximumLength(300)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));

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
