using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Producer.Farm.Select
{
    public class FarmImageSelectDto : BaseDto
    {

        public FarmImageSelectDto(int id, string fileName, string imageUrl, string publicId, int farmId)
        {
            Id = id;
            FileName = fileName;
            ImageUrl = imageUrl;
            PublicId = publicId;
            FarmId = farmId;
        }

        //public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string FileName { get; set; }
        public string PublicId { get; set; }
        public int FarmId { get; set; }
    }
}
