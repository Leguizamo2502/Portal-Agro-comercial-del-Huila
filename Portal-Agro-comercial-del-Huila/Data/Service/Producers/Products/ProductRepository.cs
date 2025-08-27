using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Producers.Products;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Producers.Products
{
    public class ProductRepository : DataGeneric<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Consulta base con el nuevo modelo:
        /// - Carga Category y ProductImages no borradas.
        /// - Carga las Fincas a través de ProductFarms, con City/Department y Producer->User->Person.
        /// </summary>
        private IQueryable<Product> BaseQuery()
        {
            return _dbSet
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(p => p.ProductFarms)
                    .ThenInclude(pf => pf.Farm)
                        .ThenInclude(f => f.City)
                            .ThenInclude(c => c.Department)
                .Include(p => p.ProductFarms)
                    .ThenInclude(pf => pf.Farm)
                        .ThenInclude(f => f.Producer)
                            .ThenInclude(prod => prod.User)
                                .ThenInclude(u => u.Person)
                .AsSplitQuery(); // evita explosión cartesiana por múltiples Includes
        }

        public async Task<IEnumerable<Product>> GetByIdsFavoritesAsync(IEnumerable<int> ids)
        {
            var idsList = ids?.Distinct().ToList() ?? new List<int>();
            if (idsList.Count == 0)
                return new List<Product>();

            return await BaseQuery()
                .Where(p => idsList.Contains(p.Id))
                .OrderByDescending(p => p.CreateAt)
                .ThenByDescending(p => p.Id)
                .ToListAsync();
        }

        public override async Task<Product> AddAsync(Product entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Add(entity);
            return await Task.FromResult(entity); // SaveChanges afuera
        }

        public override async Task<bool> UpdateAsync(Product entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var existing = await _dbSet
                .Include(e => e.ProductImages)
                .FirstOrDefaultAsync(e => e.Id == entity.Id && !e.IsDeleted);

            if (existing == null)
                throw new InvalidOperationException($"No se encontró el producto con ID {entity.Id}.");

            _context.Entry(existing).CurrentValues.SetValues(entity);

            // Sincronización de imágenes (se mantiene igual)
            var imagesToRemove = existing.ProductImages
                .Where(img => !entity.ProductImages.Any(eImg => eImg.Id == img.Id))
                .ToList();
            foreach (var img in imagesToRemove)
                _context.Set<ProductImage>().Remove(img);

            var imagesToAdd = entity.ProductImages.Where(img => img.Id == 0).ToList();
            foreach (var img in imagesToAdd)
            {
                img.ProductId = existing.Id;
                existing.ProductImages.Add(img);
            }

            return true; // SaveChanges afuera
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await BaseQuery()
                .OrderByDescending(p => p.CreateAt)
                .ThenByDescending(p => p.Id)
                .ToListAsync();
        }

        public override async Task<Product?> GetByIdAsync(int id)
        {
            var product = await BaseQuery()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                // Por si acaso, reforzamos el filtro de imágenes no borradas
                product.ProductImages = product.ProductImages
                    .Where(pi => !pi.IsDeleted)
                    .ToList();
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetByProducer(int? producerId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.ProducerId == producerId)
                .OrderByDescending(p => p.CreateAt)
                .ThenByDescending(p => p.Id)
                .Include(p => p.Category)
                .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(p => p.ProductFarms)
                    .ThenInclude(pf => pf.Farm)
                        .ThenInclude(f => f.City)
                            .ThenInclude(c => c.Department)
                .Include(p => p.ProductFarms)
                    .ThenInclude(pf => pf.Farm)
                        .ThenInclude(f => f.Producer)
                            .ThenInclude(prod => prod.User)
                                .ThenInclude(u => u.Person)
                .AsSplitQuery()
                .ToListAsync();
        }
    }
}
