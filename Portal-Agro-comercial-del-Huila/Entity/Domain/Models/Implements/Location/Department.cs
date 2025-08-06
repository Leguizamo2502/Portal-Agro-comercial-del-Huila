
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Location
{
    public class Department : BaseModel
    {
        public string Name { get; set; }

        public List<City> Cities { get; set; } = new();
    }
}
