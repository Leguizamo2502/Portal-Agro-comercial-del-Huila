using Data.Interfaces.Implements.Orders;
using Data.Repository;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Orders;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Orders
{
    public class OrderRepository : DataGeneric<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Order> AddAsync(Order entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Add(entity);
            return await Task.FromResult(entity); // SaveChanges afuera
        }


        public async Task<bool> UpdateOrderAsync(Order entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var existing = await _dbSet
                .FirstOrDefaultAsync(e => e.Id == entity.Id && !e.IsDeleted);

            if (existing == null)
                throw new InvalidOperationException($"No se encontró la orden con ID {entity.Id}.");

            _context.Entry(existing).CurrentValues.SetValues(entity);

            
            return true; // SaveChanges afuera
        }

        // Todos los pedidos del productor (activos y no eliminados)
        public async Task<IEnumerable<Order>> GetOrdersByProducer(int producerId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(o => o.ProducerIdSnapshot == producerId && !o.IsDeleted && o.Active)
                .OrderByDescending(o => o.CreateAt)
                .ToListAsync();
        }

        // Solo pedidos pendientes de revisión del productor
        public async Task<IEnumerable<Order>> GetPendingOrdersByProducer(int producerId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(o => o.ProducerIdSnapshot == producerId
                            && !o.IsDeleted
                            && o.Active
                            && o.Status == OrderStatus.PendingReview)
                .OrderByDescending(o => o.CreateAt)
                .ToListAsync();
        }
    }
}
