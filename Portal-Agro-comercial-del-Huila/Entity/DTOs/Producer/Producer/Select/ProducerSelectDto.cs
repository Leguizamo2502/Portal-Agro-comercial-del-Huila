using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Producer.Producer.Select
{
    public class ProducerSelectDto : BaseDto
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? QrUrl { get; set; }
        public string Description { get; set; }
        public double AverageRating { get; set; }
        public List<ProducerSocialReadDto> Networks { get; set; } = new();

    }
}
