using Business.Interfaces.Implements.Producers.Analitics;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.Implements.Producers.Analytics;
using Entity.DTOs.Producer.Analytics;

namespace Business.Services.Producers.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {

        private readonly IAnalyticsReadRepository _analyticsRead;
        private readonly IProducerRepository _producerRead;

        public AnalyticsService(IAnalyticsReadRepository analyticsRead, IProducerRepository producerRead)
        {
            _analyticsRead = analyticsRead;
            _producerRead = producerRead;
        }

        public async Task<List<TopProductStatDto>> GetTopProductsForCurrentProducerAsync(int userId, int limit, CancellationToken ct = default)
        {
            if (userId <= 0) throw new ArgumentException("userId inválido.", nameof(userId));

            var producerId = await _producerRead.GetIdProducer(userId);
            return producerId is null
                ? throw new InvalidOperationException("El usuario autenticado no tiene productor asociado.")
                : await _analyticsRead.GetTopProductsByCompletedOrdersAsync(producerId.Value, limit, ct);
        }
    }
}
