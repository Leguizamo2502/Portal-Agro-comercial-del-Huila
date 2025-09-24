using Entity.DTOs.Producer.Analytics;

namespace Business.Interfaces.Implements.Producers.Analitics
{
    public interface IAnalyticsService
    {
        Task<List<TopProductStatDto>> GetTopProductsForCurrentProducerAsync(int userId, int limit, CancellationToken ct = default);
    }
}
