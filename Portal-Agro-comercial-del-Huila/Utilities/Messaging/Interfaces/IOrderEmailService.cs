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

        Task SendOrderAcceptedAwaitingPaymentToCustomer(
            string emailReceptor,
            int orderId,
            string productName,
            int quantityRequested,
            decimal total,
            DateTime acceptedAtUtc,
            DateTime paymentDeadlineUtc
        );

        Task SendPaymentSubmittedToProducer(
            string emailReceptor, int orderId, string productName, int quantityRequested,
            decimal total, DateTime uploadedAtUtc);

        Task SendOrderExpiredByNoPaymentToCustomer(
           string emailReceptor,
           int orderId,
           string productName,
           int quantityRequested,
           decimal total,
           DateTime expiredAtUtc);

        // === Nuevos (opcionales) ===
        Task SendOrderPreparingToCustomer(
            string emailReceptor, int orderId, string productName, int quantityRequested,
            decimal total, DateTime preparingAtUtc);

        Task SendOrderDispatchedToCustomer(
            string emailReceptor, int orderId, string productName, int quantityRequested,
            decimal total, DateTime dispatchedAtUtc);

        Task SendOrderDeliveredToCustomer(
            string emailReceptor, int orderId, string productName, int quantityRequested,
            decimal total, DateTime deliveredAtUtc);

        Task SendOrderCancelledByUserToProducer(
            string emailReceptor, int orderId, string productName, int quantityRequested,
            DateTime cancelledAtUtc);

        Task SendOrderCompletedToCustomer(
           string emailReceptor,
           int orderId,
           string productName,
           int quantityRequested,
           decimal total,
           DateTime completedAtUtc,
           bool autoCompleted = false // true si lo cerró el job automáticamente
       );
    }
}
