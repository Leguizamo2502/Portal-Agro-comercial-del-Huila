using Data.Interfaces.Implements.Producers.Farms;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Products;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Producers.Farms
{
    public class FarmRepository : DataGeneric<Farm>, IFarmRepository
    {
        public FarmRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<Farm> AddAsync(Farm entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);
            // No SaveChanges aquí

            return await Task.FromResult(entity);
        }

        public override async Task<bool> UpdateAsync(Farm entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var existing = await _dbSet
                .Include(e => e.FarmImages)
                .FirstOrDefaultAsync(e => e.Id == entity.Id && !e.IsDeleted);

            if (existing == null)
                throw new InvalidOperationException($"No se encontró el  con ID {entity.Id}.");

            _context.Entry(existing).CurrentValues.SetValues(entity);

            // Sincronización imágenes
            var imagesToRemove = existing.FarmImages.Where(img => !entity.FarmImages.Any(eImg => eImg.Id == img.Id)).ToList();
            foreach (var img in imagesToRemove)
                _context.Set<FarmImage>().Remove(img);

            var imagesToAdd = entity.FarmImages.Where(img => img.Id == 0).ToList();
            foreach (var img in imagesToAdd)
            {
                img.FarmId = existing.Id;
                existing.FarmImages.Add(img);
            }

            // No SaveChanges aquí
            return existing != null;
        }





        public override async Task<IEnumerable<Farm>> GetAllAsync()
        {
            return await _dbSet
                .Include(f => f.City)
                    .ThenInclude(c => c.Department)
                .Include(f => f.Producer)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.Person)
                .Include(f => f.FarmImages.Where(pi => !pi.IsDeleted))
                .Where(f  => !f.IsDeleted)
                .ToListAsync();
        }

        public override async Task<Farm?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(f => f.City)
                    .ThenInclude(c => c.Department)
                .Include(f => f.Producer)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.Person)
                .Include(f => f.FarmImages.Where(pi => !pi.IsDeleted))
                .FirstOrDefaultAsync(f=> f.Id == id);
        }

        public async Task<IEnumerable<Farm>> GetByProducer(int? producerId)
        {
            return await _dbSet
                .Include(f => f.City)
                    .ThenInclude(c => c.Department)
                .Include(f => f.Producer)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.Person)
                .Include(f => f.FarmImages.Where(pi => !pi.IsDeleted))
                .Where(f => f.Producer.Id == producerId && !f.IsDeleted)
                .ToListAsync();
        }
    }
}
