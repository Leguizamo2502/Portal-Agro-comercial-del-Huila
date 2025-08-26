using Entity.DTOs.Auth;
using FluentValidation;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        // Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio.")
            .Must(NotWhiteSpace).WithMessage("El email no puede estar en blanco.")
            .EmailAddress().WithMessage("Formato de email no válido.")
            .MaximumLength(150).WithMessage("El email no debe superar 150 caracteres.");

        // Password
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            //.MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            //.Matches("[A-Z]").WithMessage("Debe contener al menos una mayúscula.")
            //.Matches("[a-z]").WithMessage("Debe contener al menos una minúscula.")
            //.Matches("[0-9]").WithMessage("Debe contener al menos un dígito.")
            //.Matches("[^a-zA-Z0-9]").WithMessage("Debe contener al menos un carácter especial.")
            .Must(p => p is null || !p.Contains(' ')).WithMessage("La contraseña no debe contener espacios.");

        // Nombre
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            //.Must(NotWhiteSpace).WithMessage("El nombre no puede estar en blanco.")
            .Length(2, 50).WithMessage("Debe tener entre 2 y 50 caracteres.")
            .Matches(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s'-]+$").WithMessage("Solo debe contener letras y espacios.");

        // Apellido
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            //.Must(NotWhiteSpace).WithMessage("El apellido no puede estar en blanco.")
            .Length(2, 50).WithMessage("Debe tener entre 2 y 50 caracteres.")
            .Matches(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s'-]+$").WithMessage("Solo debe contener letras y espacios.");

        // Identificación
        RuleFor(x => x.Identification)
            .NotEmpty().WithMessage("La identificación es obligatoria.")
            .Must(NotWhiteSpace).WithMessage("La identificación no puede estar en blanco.")
            .Length(5, 20).WithMessage("Debe tener entre 5 y 20 caracteres.")
            .Matches(@"^[A-Za-z0-9.\-]+$").WithMessage("Solo puede contener letras, números, punto y guion.");

        // Teléfono
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .Must(NotWhiteSpace).WithMessage("El teléfono no puede estar en blanco.")
            .Must(HasValidPhoneDigits).WithMessage("Debe tener entre 7 y 15 dígitos.")
            .MaximumLength(25).WithMessage("No debe superar 25 caracteres.");

        // Dirección
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("La dirección es obligatoria.")
            //.Must(NotWhiteSpace).WithMessage("La dirección no puede estar en blanco.")
            .Length(5, 120).WithMessage("Debe tener entre 5 y 120 caracteres.");

        // Ciudad
        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage("Debe seleccionar una ciudad válida.");
    }

    // Helpers
    private static bool NotWhiteSpace(string? s) =>
        !string.IsNullOrWhiteSpace(s);

    private static bool HasValidPhoneDigits(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return false;
        int digits = 0;
        foreach (var ch in phone)
            if (char.IsDigit(ch)) digits++;
        return digits >= 7 && digits <= 15;
    }
}
