using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Validations.interfaces
{
    public interface IValidatorService
    {
        Task ValidateAsync<T>(T instance, CancellationToken ct = default);
    }
}
