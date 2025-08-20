using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers.Products;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Products;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Data.Service.Producers.Products
{
    public class ProductRepository : DataGeneric<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }



        public override async Task<Product> AddAsync(Product entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);
            // No SaveChanges aquí

            return await Task.FromResult(entity);
        }


        public override async Task<bool> UpdateAsync(Product entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var existing = await _dbSet
                .Include(e => e.ProductImages)
                .FirstOrDefaultAsync(e => e.Id == entity.Id && !e.IsDeleted);

            if (existing == null)
                throw new InvalidOperationException($"No se encontró el  con ID {entity.Id}.");

            _context.Entry(existing).CurrentValues.SetValues(entity);

            // Sincronización imágenes
            var imagesToRemove = existing.ProductImages.Where(img => !entity.ProductImages.Any(eImg => eImg.Id == img.Id)).ToList();
            foreach (var img in imagesToRemove)
                _context.Set<ProductImage>().Remove(img);

            var imagesToAdd = entity.ProductImages.Where(img => img.Id == 0).ToList();
            foreach (var img in imagesToAdd)
            {
                img.ProductId = existing.Id;
                existing.ProductImages.Add(img);
            }

            // No SaveChanges aquí
            return existing != null;
        }


        public override async Task<IEnumerable<Product>> GetAllAsync()
        {

            return await _dbSet
               .AsNoTracking()
               .OrderByDescending(p => p.CreateAt)          // último creado primero
               .ThenByDescending(p => p.Id)                  // desempate estable
               .Include(p => p.Category)
               .Include(p => p.Farm)
                   .ThenInclude(f => f.City)
                       .ThenInclude(c => c.Department)
               .Include(p => p.Farm)
                   .ThenInclude(f => f.Producer)
                       .ThenInclude(prod => prod.User)
                           .ThenInclude(u => u.Person)
               .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
               .ToListAsync();

        }

        public override async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _dbSet
             .AsNoTracking()
             .Include(p => p.Category)
             .Include(p => p.Farm)
                 .ThenInclude(f => f.City)
                     .ThenInclude(c => c.Department)
             .Include(p => p.Farm)
                 .ThenInclude(f => f.Producer)
                     .ThenInclude(prod => prod.User)
                         .ThenInclude(u => u.Person)
             .Include(p => p.ProductImages)
             .FirstOrDefaultAsync(p => p.Id == id);

                    if (product != null)
                    {
                        product.ProductImages = product.ProductImages
                            .Where(pi => !pi.IsDeleted)
                            .ToList();
                    }

            return product;

        }

        public async Task<IEnumerable<Product>> GetByProducer(int producerId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Farm)
                    .ThenInclude(f => f.City)
                        .ThenInclude(c => c.Department)
                .Include(p => p.Farm)
                    .ThenInclude(f => f.Producer)
                        .ThenInclude(prod => prod.User)
                            .ThenInclude(u => u.Person)
                .Include(p => p.ProductImages)
                .Where(p=>p.Farm.Producer.Id == producerId)
                .ToListAsync();
        }
    }
}
