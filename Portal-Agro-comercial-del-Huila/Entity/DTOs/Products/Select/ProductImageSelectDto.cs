using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.BaseDTO;

namespace Entity.DTOs.Products.Select
{
    public class ProductImageSelectDto : BaseDto
    {
        public ProductImageSelectDto(int id, string fileName, string imageUrl, string publicId, int productId)
        {
            Id = id;
            FileName = fileName;
            ImageUrl = imageUrl;
            PublicId = publicId;
            ProductId = productId;
        }

        //public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string FileName { get; set; } 
        public string PublicId { get; set; }
        public int ProductId { get; set; }

    }
}
