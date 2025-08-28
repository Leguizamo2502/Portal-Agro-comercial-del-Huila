using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Order.Reviews
{
    public class ReviewCreateDto : BaseDto
    {
        public int ProductId { get; set; }
        public byte Rating { get; set; }      // 1..5
        public string Comment { get; set; }   // requerido
    }
}
