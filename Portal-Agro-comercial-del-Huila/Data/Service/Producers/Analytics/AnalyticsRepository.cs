using Data.Interfaces.Implements.Producers.Analytics;
using Entity.Domain.Enums;
using Entity.DTOs.Producer.Analytics;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Producers.Analytics
{
    public class AnalyticsRepository : IAnalyticsReadRepository
    {
        private readonly ApplicationDbContext _context;
        public AnalyticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TopProductStatDto>> GetTopProductsByCompletedOrdersAsync(int producerId, int limit, CancellationToken ct = default)
        {
            if (limit <= 0) limit = 5;
            if (limit > 20) limit = 20;

            var query =
                from o in _context.Orders.AsNoTracking()
                where o.ProducerIdSnapshot == producerId
                      && o.Status == OrderStatus.Completed
                group o by new { o.ProductId, o.ProductNameSnapshot } into g
                orderby g.Count() descending, g.Sum(x => x.QuantityRequested) descending
                select new TopProductStatDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductNameSnapshot,
                    CompletedOrders = g.Count(),
                    TotalUnits = g.Sum(x => x.QuantityRequested),
                    TotalRevenue = g.Sum(x => x.QuantityRequested * x.UnitPriceSnapshot)
                };

            return await query.Take(limit).ToListAsync(ct);
        }
    }
}
