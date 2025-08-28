using Entity.DTOs.Producer.Farm.Update;
using FluentValidation;

public class FarmUpdateDtoValidator : AbstractValidator<FarmUpdateDto>
{
    public FarmUpdateDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        // Id heredado de BaseDto
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id inválido.");

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

        // Imágenes nuevas: opcionales
        RuleFor(x => x.Images)
            .Must(list => list == null || list.Count <= 5)
            .WithMessage("Máximo 5 imágenes nuevas.");

        //RuleForEach(x => x.Images).ChildRules(img =>
        //{
        //    img.RuleFor(i => i.Length)
        //        .LessThanOrEqualTo(2 * 1024 * 1024).WithMessage("Cada imagen debe pesar máximo 2MB.");

        //    img.RuleFor(i => i.ContentType)
        //        .Must(t => t == "image/jpeg" || t == "image/png")
        //        .WithMessage("Solo se permiten imágenes JPEG o PNG.");
        //});

        // Imágenes a eliminar: opcionales, pero válidas si llegan
        //RuleForEach(x => x.ImagesToDelete)
        //    .NotEmpty().WithMessage("Id de imagen a eliminar inválido.");
    }
}
