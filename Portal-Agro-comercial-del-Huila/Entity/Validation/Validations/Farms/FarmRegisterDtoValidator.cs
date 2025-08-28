using Entity.DTOs.Producer.Farm.Create;
using FluentValidation;
using Microsoft.AspNetCore.Http;

public class FarmRegisterDtoValidator : AbstractValidator<FarmRegisterDto>
{
    public FarmRegisterDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la finca es obligatorio.")
            .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres.");

        RuleFor(x => x.Hectares)
            .GreaterThan(0).WithMessage("Las hectáreas deben ser mayores a 0.");

        RuleFor(x => x.Altitude)
            .GreaterThanOrEqualTo(0).WithMessage("La altitud no puede ser negativa.")
            .LessThanOrEqualTo(9000).WithMessage("La altitud no debe superar 9000 msnm.");

        //RuleFor(x => x.Latitude)
        //    .InclusiveBetween(-90, 90).WithMessage("Latitud fuera de rango (-90 a 90).");

        //RuleFor(x => x.Longitude)
        //    .InclusiveBetween(-180, 180).WithMessage("Longitud fuera de rango (-180 a 180).");

        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage("Debe seleccionar una ciudad válida.");

        //RuleFor(x => x.ProducerId)
        //    .GreaterThan(0).WithMessage("Debe seleccionar un productor válido.");

        RuleFor(x => x.Images)
            .NotNull().WithMessage("Debe adjuntar al menos una imagen.")
            .Must(i => i.Any()).WithMessage("Debe adjuntar al menos una imagen.")
            .Must(i => i.Count <= 5).WithMessage("No puede adjuntar más de 5 imágenes.");

        //RuleForEach(x => x.Images).ChildRules(img =>
        //{
        //    img.RuleFor(i => i.Length)
        //        .LessThanOrEqualTo(2 * 1024 * 1024).WithMessage("Cada imagen debe pesar máximo 2MB.");

        //    img.RuleFor(i => i.ContentType)
        //        .Must(t => t == "image/jpeg" || t == "image/png")
        //        .WithMessage("Solo se permiten imágenes JPEG o PNG.");
        //});
    }
}
