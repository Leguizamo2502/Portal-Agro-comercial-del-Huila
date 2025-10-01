namespace Entity.Domain.Models.Base
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public bool Active { get; set; } = true;
    }
}
