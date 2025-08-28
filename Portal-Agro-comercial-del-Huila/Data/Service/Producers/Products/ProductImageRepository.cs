using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers.Products;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
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

        //public Task AddAsync(List<Images> images)
        //{
        //    if (images == null || !images.Any())
        //        return Task.CompletedTask;

        //    // No transacciones ni SaveChanges aquí
        //    _dbSet.AddRange(images);
        //    return Task.CompletedTask;
        //}

        public async Task<List<ProductImage>> GetByProductIdAsync(int productId)
        {
            return await _dbSet
                .Where(e => e.ProductId == productId && !e.IsDeleted)
                .ToListAsync();
        }


      

        public async Task<bool> DeleteByPublicIdAsync(string publicId)
        {
            var image = await _dbSet.FirstOrDefaultAsync(i => i.PublicId == publicId);
            if (image == null) return false;

            _dbSet.Remove(image);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteLogicalByPublicIdAsync(string publicId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(i => i.PublicId == publicId);
            if (entity == null) return false;

            entity.Active = false;
            entity.IsDeleted = true; // o el nombre del flag soft delete
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
