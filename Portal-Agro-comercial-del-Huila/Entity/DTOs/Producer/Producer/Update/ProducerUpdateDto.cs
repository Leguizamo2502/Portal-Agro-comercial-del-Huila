using Entity.DTOs.BaseDTO;
using Entity.DTOs.Producer.Producer.Create;

namespace Entity.DTOs.Producer.Producer.Update
{
    public class ProducerUpdateDto
    {
        public string Description { get; set; } = string.Empty;
        public List<ProducerSocialCreateDto>? SocialLinks { get; set; }
    }
}
