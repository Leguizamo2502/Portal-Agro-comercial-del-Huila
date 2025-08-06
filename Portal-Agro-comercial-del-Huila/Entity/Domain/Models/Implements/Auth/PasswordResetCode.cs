using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Auth
{
    public  class PasswordResetCode : BaseModel
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
