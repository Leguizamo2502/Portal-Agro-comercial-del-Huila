using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Security.Selects.Rols
{
    public class RolSelectDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
