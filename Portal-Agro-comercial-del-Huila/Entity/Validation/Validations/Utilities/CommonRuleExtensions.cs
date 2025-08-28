using FluentValidation;

public static class CommonRuleExtensions
{
    public static IRuleBuilderOptions<T, string> NameRules<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("El nombre es obligatorio.")
            //.Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage("El nombre no puede estar en blanco.")
            .Length(5, 100).WithMessage("El nombre debe tener entre 5 y 100 caracteres.");

    public static IRuleBuilderOptions<T, string> DescriptionRules<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("La descripción es obligatoria.")
            //.Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage("La descripción no puede estar en blanco.")
            .Length(10, 300).WithMessage("La descripción debe tener entre 10 y 300 caracteres.");
}
