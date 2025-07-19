
using Entity.Domain.Models.Implements.Auth;

namespace Entity.Domain.Models.Implements.Location
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<Person>? People { get; set; } = new();
    }
}
