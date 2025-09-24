using Entity.Domain.Enums;
using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Producer.Producer.Select
{
    public class ProducerSocialReadDto : BaseDto
    {
        public SocialNetwork Network { get; set; }
        public string Url { get; set; }

    }
}
