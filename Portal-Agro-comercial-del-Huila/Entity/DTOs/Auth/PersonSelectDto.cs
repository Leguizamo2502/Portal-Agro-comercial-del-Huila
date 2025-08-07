using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Auth
{
    public class PersonSelectDto : BaseDto
    {
        public string FullName { get; set; }
        //public string LastName { get; set; }
        public string Identification { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int CityId { get; set; }
    }
}
