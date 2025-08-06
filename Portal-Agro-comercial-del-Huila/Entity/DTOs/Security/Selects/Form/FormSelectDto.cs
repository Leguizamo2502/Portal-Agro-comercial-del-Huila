using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Security.Selects.Rols
{
    public class FormSelectDto : BaseDto
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
