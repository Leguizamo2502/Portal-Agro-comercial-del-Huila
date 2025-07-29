using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Location;

namespace Entity.Domain.Models.Implements.Auth
{
    public class Person : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public int CityId { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }


        // Navegación inversa
        public City City { get; set; }

        public User User { get; set; }
    }
}
