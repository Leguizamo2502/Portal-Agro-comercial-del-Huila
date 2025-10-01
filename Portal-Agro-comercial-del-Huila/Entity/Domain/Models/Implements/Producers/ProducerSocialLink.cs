using Entity.Domain.Enums;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Producers
{
    public class ProducerSocialLink : BaseModel
    {
        public int ProducerId { get; set; }
        public Producer Producer { get; set; } = default!;

        public SocialNetwork Network { get; set; }
        public string Url { get; set; } = default!;
    }
}
