using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Auth
{
    public class RequestResetDto
    {
        public string Email { get; set; } = null!;
    }
}
