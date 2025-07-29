using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers.Products;
using Data.Repository;
using Entity.Domain.Models.Implements.Products;
using Entity.Infrastructure.Context;

namespace Data.Service.Producers.Products
{
    public class ProductImageRepository : DataGeneric<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
