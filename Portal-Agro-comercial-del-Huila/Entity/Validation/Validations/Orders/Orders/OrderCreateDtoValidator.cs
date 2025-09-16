using Entity.DTOs.Order.Create;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Entity.Validation.Validations.Orders.Orders
{
    public class CreateOrderDtoValidator : AbstractValidator<OrderCreateDto>
    {

        public CreateOrderDtoValidator()
        {
            // Producto
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("El producto es obligatorio.");

            // Cantidad
            RuleFor(x => x.QuantityRequested)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");

           

            // Nombre del destinatario
            RuleFor(x => x.RecipientName)
                .NotEmpty().WithMessage("El nombre del destinatario es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            // Teléfono de contacto
            RuleFor(x => x.ContactPhone)
                .NotEmpty().WithMessage("El teléfono de contacto es obligatorio.")
                .MaximumLength(30).WithMessage("El teléfono no puede superar los 30 caracteres.");

            // Dirección
            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(200).WithMessage("La dirección no puede superar los 200 caracteres.");

            // Ciudad
            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage("La ciudad es obligatoria.");

            // Notas adicionales (opcionales, pero con límite)
            RuleFor(x => x.AdditionalNotes)
                .MaximumLength(500).WithMessage("Las notas adicionales no pueden superar los 500 caracteres.");
        }

        private bool BeAValidImage(IFormFile? file)
        {
            if (file == null) return false;

            var allowed = new[] { "image/jpeg", "image/png", "image/jpg" };
            if (!allowed.Contains(file.ContentType.ToLower())) return false;

            const long maxSize = 5 * 1024 * 1024; // 5 MB
            if (file.Length > maxSize) return false;

            return true;
        }
    }
}
