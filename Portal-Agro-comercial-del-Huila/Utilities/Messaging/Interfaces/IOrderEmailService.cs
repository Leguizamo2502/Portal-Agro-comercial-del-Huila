namespace Utilities.Messaging.Interfaces
{
    public interface IOrderEmailService
    {
        
            Task SendOrderCreatedEmail(
            string emailReceptor,
            int orderId,
            string productName,
            int quantityRequested,
            decimal subtotal,
            decimal total,
            DateTime createdAtUtc,
            string? personName = null,
            string? counterpartName = null,
            bool isProducer = false
            );
        Task SendOrderAcceptedToCustomer(string emailReceptor, int orderId, string productName, int quantityRequested, decimal total, DateTime decisionAtUtc);
        Task SendOrderRejectedToCustomer(string emailReceptor, int orderId, string productName, int quantityRequested, string reason, DateTime decisionAtUtc);
        Task SendOrderCompletedToProducer(string emailReceptor, int orderId, string productName, int quantityRequested, decimal total, DateTime completedAtUtc);
        Task SendOrderDisputedToProducer(string emailReceptor, int orderId, string productName, int quantityRequested, decimal total, DateTime disputedAtUtc);

    }
}
