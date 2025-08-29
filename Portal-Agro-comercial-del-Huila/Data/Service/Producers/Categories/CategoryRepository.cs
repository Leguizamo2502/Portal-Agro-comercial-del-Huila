using Data.Interfaces.Implements.Producers.Categories;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Producers.Categories
{
    public class CategoryRepository : DataGeneric<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Include(c => c.SubCategories)
                .Where(c=>c.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetNodesAsync(int? parentId)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(c => !c.IsDeleted && c.Active);

            query = parentId is null
                ? query.Where(c => c.ParentCategoryId == null)          // raíces
                : query.Where(c => c.ParentCategoryId == parentId);     // hijos

            return await query
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetChildrenCountByParentsAsync(IEnumerable<int> parentIds)
        {
            var ids = parentIds?.Distinct().ToList() ?? new List<int>();
            if (ids.Count == 0) return new Dictionary<int, int>();

            return await _dbSet
                .AsNoTracking()
                .Where(c => !c.IsDeleted && c.Active && c.ParentCategoryId != null && ids.Contains(c.ParentCategoryId.Value))
                .GroupBy(c => c.ParentCategoryId!.Value)
                .Select(g => new { ParentId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ParentId, x => x.Count);
        }


    }
}
