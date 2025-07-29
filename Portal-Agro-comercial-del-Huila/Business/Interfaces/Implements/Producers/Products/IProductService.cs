using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.IBusiness;
using Business.Repository;
using Entity.DTOs.Products;

namespace Business.Interfaces.Implements.Producers.Products
{
    public interface IProductService : IBusiness<ProductCreateDto,ProductSelectDto>
    {
    }
}
