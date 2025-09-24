using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Auth.User
{
    public class UserSelectDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CityName { get; set; }
        public bool Active { get; set; }
        public int CityId { get; set; }
        public int DepartmentId { get; set; }
        public IEnumerable<string> Roles { get; set; } = [];
    }
}
