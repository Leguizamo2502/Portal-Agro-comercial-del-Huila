using Entity.Validations.interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Entity.Validation.Service
{
    public class ValidatorService : IValidatorService
    {
        private readonly IServiceProvider _provider;

        public ValidatorService(IServiceProvider provider)
        {
            _provider = provider;
        }
        public async Task ValidateAsync<T>(T instance, CancellationToken ct = default)
        {
            var validator = _provider.GetService<IValidator<T>>();
            if (validator is null)
                throw new InvalidOperationException($"No hay validador para {typeof(T).Name}");

            var result = await validator.ValidateAsync(instance, ct);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }
    }
}
