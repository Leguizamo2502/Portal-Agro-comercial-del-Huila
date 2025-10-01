using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Producers.Products;

namespace Entity.Domain.Models.Implements.Producers
{
    public class Producer : BaseModel
    {
        public string Code { get; set; } = default!;
        public string? QrUrl { get; set; }
        public string Description { get; set; } = default!;
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        public ICollection<Farm> Farms { get; set; } = [];
        public ICollection<Product> Products { get; set; } = [];
        public ICollection<ProducerSocialLink> SocialLinks { get; set; } = [];

    }
}
