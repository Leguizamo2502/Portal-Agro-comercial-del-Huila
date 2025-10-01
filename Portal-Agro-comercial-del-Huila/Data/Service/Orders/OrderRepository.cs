using Data.Interfaces.Implements.Orders;
using Data.Repository;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Orders;
using Entity.Domain.Models.Implements.Producers;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Orders
{
    public class OrderRepository : DataGeneric<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Order> AddAsync(Order entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Add(entity);
            // SaveChanges se hace afuera (servicio)
            return await Task.FromResult(entity);
        }

        public async Task<bool> UpdateOrderAsync(Order entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            // Trackeada (sin AsNoTracking) para poder aplicar concurrencia
            var existing = await _dbSet
                .FirstOrDefaultAsync(e => e.Id == entity.Id && !e.IsDeleted);

            if (existing == null)
                throw new InvalidOperationException($"No se encontró la orden con ID {entity.Id}.");

            var entry = _context.Entry(existing);

            // Fijar RowVersion original para control de concurrencia (si la usas en el modelo)
            // Nota: 'entity.RowVersion' debe venir poblada desde capa superior (por ejemplo, cuando leíste la orden previamente)
            if (entity.RowVersion is not null && entity.RowVersion.Length > 0)
            {
                entry.Property(x => x.RowVersion).OriginalValue = entity.RowVersion;
            }

            // Copiar valores excepto RowVersion
            entry.CurrentValues.SetValues(entity);
            entry.Property(x => x.RowVersion).IsModified = false;

            // SaveChanges afuera
            return true;
        }

        public override async Task<Order?> GetByIdAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(o => o.City)
                    .ThenInclude(c => c.Department)
                .FirstOrDefaultAsync(o => o.IsDeleted == false && o.Active == true && o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByProducerAsync(int producerId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(o=>o.City)
                    .ThenInclude(c=>c.Department)
                .Where(o => !o.IsDeleted && o.Active
                            && o.ProducerIdSnapshot == producerId)
                .OrderByDescending(o => o.CreateAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersByProducerAsync(int producerId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(o => o.City)
                    .ThenInclude(c => c.Department)
                .Where(o => !o.IsDeleted && o.Active
                            && o.ProducerIdSnapshot == producerId
                            && o.Status == OrderStatus.PendingReview)
                .OrderByDescending(o => o.CreateAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _dbSet.AsNoTracking()
                .Include(o => o.City)
                    .ThenInclude(c => c.Department)
               .Where(o => o.UserId == userId && !o.IsDeleted && o.Active)
               .OrderByDescending(o => o.CreateAt)
               .ToListAsync();
        }

        public async Task<Order?> GetByCode(string code)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(o => o.City)
                    .ThenInclude(c => c.Department)
                .FirstOrDefaultAsync(o => o.IsDeleted == false && o.Active == true && o.Code == code);
        }
    }
}
