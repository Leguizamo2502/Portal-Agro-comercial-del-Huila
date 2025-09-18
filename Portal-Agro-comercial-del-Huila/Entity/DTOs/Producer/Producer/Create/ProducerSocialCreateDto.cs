using Entity.Domain.Enums;

namespace Entity.DTOs.Producer.Producer.Create
{
    public class ProducerSocialCreateDto
    {
        public SocialNetwork Network { get; set; }
        public string Url { get; set; }
    }
}
