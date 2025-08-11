using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers.Products;
using Data.Repository;
using Entity.Domain.Models.Implements.Products;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Producers.Products
{
    public class ProductRepository : DataGeneric<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {

            return await _dbSet
                .AsNoTracking()
                .Include(p=>p.Category)
                .Include(p => p.Farm)
                    .ThenInclude(f => f.City)
                        .ThenInclude(c => c.Department)
                .Include(p => p.Farm)
                    .ThenInclude(f => f.Producer)
                        .ThenInclude(prod => prod.User)
                            .ThenInclude(u => u.Person)
                .Include(p => p.ProductImages)
                .ToListAsync();

        }
    }
}
