using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Order.Reviews
{
    public class ReviewSelectDto : BaseDto
    {
        //public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public byte Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
