
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Producers;

namespace Entity.Domain.Models.Implements.Location
{
    public class City : BaseModel
    {
        
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Person>? People { get; set; } = [];
        public ICollection<Farm>? Farms { get; set; } = [];


    }
}
