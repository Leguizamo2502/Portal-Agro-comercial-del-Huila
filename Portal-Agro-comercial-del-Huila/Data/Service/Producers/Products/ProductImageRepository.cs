using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers.Products;
using Data.Repository;
using Entity.Domain.Models.Implements.Products;
using Entity.Infrastructure.Context;
using static System.Net.Mime.MediaTypeNames;

namespace Data.Service.Producers.Products
{
    public class ProductImageRepository : DataGeneric<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task AddImages(List<ProductImage> images)
        {
            if (images == null || !images.Any())
                return Task.CompletedTask;

            // No transacciones ni SaveChanges aquí
            _dbSet.AddRange(images);
            return Task.CompletedTask;
        }
    }
}
