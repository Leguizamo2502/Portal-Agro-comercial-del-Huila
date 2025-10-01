using Entity.DTOs.Producer.Analytics;

namespace Data.Interfaces.Implements.Producers.Analytics
{
    public interface IAnalyticsReadRepository
    {
        Task<List<TopProductStatDto>> GetTopProductsByCompletedOrdersAsync(int producerId, int limit, CancellationToken ct = default);
    }
}
