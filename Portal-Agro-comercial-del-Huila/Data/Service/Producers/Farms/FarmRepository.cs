using System.Linq;
using Data.Interfaces.Implements.Producers.Farms;
using Data.Repository;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Producers.Farms;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Producers.Farms
{
    public class FarmRepository : DataGeneric<Farm>, IFarmRepository
    {
        public FarmRepository(ApplicationDbContext context) : base(context) { }

        // -----------------------
        // Base de consulta común
        // -----------------------
        private IQueryable<Farm> BaseQuery(bool includeImages = false, bool includeDeleted = false)
        {
            IQueryable<Farm> query = _dbSet
                .AsNoTracking()
                .Include(f => f.City)
                    .ThenInclude(c => c.Department)
                .Include(f => f.Producer)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.Person);

            if (includeImages)
                query = query.Include(f => f.FarmImages.Where(pi => !pi.IsDeleted)); // OK

            if (!includeDeleted)
                query = query.Where(f => !f.IsDeleted);

            return query.AsSplitQuery();
        }


        // -----------------------
        // Writes
        // -----------------------
        // Si no haces await, no uses async: evitas la state machine
        public override Task<Farm> AddAsync(Farm entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Add(entity);
            // No SaveChanges aquí
            return Task.FromResult(entity);
        }

        public override async Task<bool> UpdateAsync(Farm entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var existing = await _dbSet
                .Include(e => e.FarmImages)
                .FirstOrDefaultAsync(e => e.Id == entity.Id && !e.IsDeleted);

            if (existing == null)
                throw new InvalidOperationException($"No se encontró la finca con ID {entity.Id}.");

            _context.Entry(existing).CurrentValues.SetValues(entity);

            // Sincronización de imágenes
            var imagesToRemove = existing.FarmImages
                .Where(img => !entity.FarmImages.Any(eImg => eImg.Id == img.Id))
                .ToList();

            foreach (var img in imagesToRemove)
            {
                // Define tu política: si usas borrado lógico en otros flujos,
                // aquí también deberías marcar IsDeleted = true en vez de Remove.
                _context.Set<FarmImage>().Remove(img);
            }

            var imagesToAdd = entity.FarmImages.Where(img => img.Id == 0).ToList();
            foreach (var img in imagesToAdd)
            {
                img.FarmId = existing.Id;
                existing.FarmImages.Add(img);
            }

            // No SaveChanges aquí
            return true;
        }

        // -----------------------
        // Reads
        // -----------------------
        public override async Task<IEnumerable<Farm>> GetAllAsync()
        {
            return await BaseQuery(includeImages: true)
                .ToListAsync();
        }

        public override async Task<Farm?> GetByIdAsync(int id)
        {
            return await BaseQuery(includeImages: true)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Farm>> GetByProducer(int? producerId)
        {
            if (producerId is null)
                return Enumerable.Empty<Farm>();

            return await BaseQuery(includeImages: true)
                .Where(f => f.ProducerId == producerId) // mejor por FK
                .ToListAsync();
        }

        public async Task<IEnumerable<Farm>> GetByProducerCode(string producerCode)
        {
            if (string.IsNullOrWhiteSpace(producerCode))
                return Enumerable.Empty<Farm>();

            return await BaseQuery(includeImages: true)
                .Where(f => f.Producer.Code == producerCode)
                .ToListAsync();
        }

        
    }
}
