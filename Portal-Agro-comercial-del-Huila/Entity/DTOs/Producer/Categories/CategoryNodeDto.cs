using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Producer.Categories
{
    public class CategoryNodeDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public bool HasChildren { get; set; }
    }
}
