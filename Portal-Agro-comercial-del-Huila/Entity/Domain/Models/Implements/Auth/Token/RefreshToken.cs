using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Models.Implements.Auth.Token
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TokenHash { get; set; } = null!; // SHA256 hex
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public string? ReplacedByTokenHash { get; set; }
        //public string? RemoteIpAddress { get; set; } // opcional para auditoría
    }
}
