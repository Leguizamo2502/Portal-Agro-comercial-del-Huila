using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Producers.Farms
{
    public class FarmImage : BaseModel
    {
        public string ImageUrl { get; set; }
        public string FileName { get; set; } = null!;
        public string PublicId { get; set; } = null!;
        public int FarmId { get; set; }

        public Farm Farm { get; set; }
    }
}
