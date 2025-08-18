using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.IBusiness;
using Business.Repository;
using Entity.Domain.Models.Implements.Products;
using Entity.DTOs.Products.Create;
using Entity.DTOs.Products.Select;
using Entity.DTOs.Products.Update;

namespace Business.Interfaces.Implements.Producers.Products
{
    public interface IProductService : IBusiness<ProductCreateDto,ProductSelectDto>
    {
        Task<IEnumerable<ProductSelectDto>> GetByProducer(int producerId);
        Task<ProductSelectDto> CreateProductAsync(ProductCreateDto dto);
        Task<ProductSelectDto> UpdateProductAsync(ProductUpdateDto dto);
    }
}
