using Data.Interfaces.Implements.Producers.Products;
using Data.Repository;
using Entity.Domain.Enums;
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
                .Include(p => p.Producer)
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

        public async Task<IEnumerable<Product>> GetAllWithLimitAsync(int? limit)
        {
            if (limit.HasValue && limit.Value > 0)
            {
                return await BaseQuery()
                    .OrderByDescending(p => p.CreateAt)
                    .ThenByDescending(p => p.Id)
                    .Take(limit.Value)
                    .ToListAsync();
            }
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
            return await BaseQuery()
                .OrderByDescending(p => p.CreateAt)
                .ThenByDescending(p => p.Id)
                .Where(p => !p.IsDeleted && p.ProducerId == producerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            if (categoryId <= 0) return Enumerable.Empty<Product>();
            return await GetByCategoriesAsync(new List<int> { categoryId }, includeDescendants: true);
        }

        public async Task<IEnumerable<Product>> GetByCategoriesAsync(List<int> categoryIds, bool includeDescendants)
        {
            if (categoryIds is null || categoryIds.Count == 0)
                return Enumerable.Empty<Product>();

            var ids = categoryIds.Where(id => id > 0).Distinct().ToList();
            if (ids.Count == 0) return Enumerable.Empty<Product>();

            IReadOnlyCollection<int> filterIds = ids;

            if (includeDescendants)
                filterIds = await GetAllDescendantCategoryIdsAsync(ids);

            return await BaseQuery()                          // ⬅️ se usa tu BaseQuery()
                .Where(p => filterIds.Contains(p.CategoryId))
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToListAsync();
        }

        private async Task<IReadOnlyCollection<int>> GetAllDescendantCategoryIdsAsync(List<int> rootIds)
        {
            var all = await _context.Category
                .AsNoTracking()
                .Where(c => !c.IsDeleted && c.Active)
                .Select(c => new { c.Id, c.ParentCategoryId })
                .ToListAsync();

            var result = new HashSet<int>(rootIds);
            var lookup = all.ToLookup(c => c.ParentCategoryId);

            var stack = new Stack<int>(rootIds);
            while (stack.Count > 0)
            {
                var parentId = stack.Pop();
                foreach (var child in lookup[parentId])
                {
                    if (result.Add(child.Id))
                        stack.Push(child.Id);
                }
            }

            return result;
        }

        public async Task<IEnumerable<Product>> GetByProducerCode(string producerCode)
        {
            return await BaseQuery()
                .OrderByDescending(p => p.CreateAt)
                .ThenByDescending(p => p.Id)
                .Where(p => !p.IsDeleted && p.Producer.Code == producerCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedAsync(int limit)
        {
            if (limit <= 0) limit = 10;

            return await BaseQuery()
                // 1) Prioriza los que tengan al menos 1 completado (true > false)
                .OrderByDescending(p => p.Orders.Any(o => !o.IsDeleted && o.Status == OrderStatus.Completed))
                // 2) Dentro de ellos, más completados primero
                .ThenByDescending(p => p.Orders.Count(o => !o.IsDeleted && o.Status == OrderStatus.Completed))
                // 3) Fallback/desempate: más recientes
                .ThenByDescending(p => p.CreateAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<bool> UpdateStock(int productId, int newStock)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(p => p.Id == productId && !p.IsDeleted);
            if (entity == null) return false;
            entity.Stock = newStock;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TryDecrementStockAsync(int productId, int quantity)
        {
            var product = await _dbSet
                .Where(p => p.Id == productId && p.Active && !p.IsDeleted)
                .FirstOrDefaultAsync();

            if (product == null)
                return false;

            if (product.Stock < quantity)
                return false; // No hay suficiente stock

            product.Stock -= quantity;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<Product?> GetByIdSmall(int id)
        {
            var product = await _dbSet
                .Include(p=>p.Producer)
                    .ThenInclude(pr => pr.User)
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
    }
}
