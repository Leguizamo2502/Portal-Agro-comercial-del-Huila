namespace Entity.Domain.Enums
{
    public enum OrderStatus
    {
        PendingReview = 0,                 // creada, sin comprobante; esperando decisión del productor
        AcceptedAwaitingPayment = 1,       // aceptada por el productor; comprador debe subir comprobante
        PaymentSubmitted = 2,              // comprador subió comprobante 
        Preparing = 3,                     // productor alista
        Dispatched = 4,                    // productor despacha (sin carrier externo)
        DeliveredPendingBuyerConfirm = 5,  // productor marcó entregado con evidencia; falta confirmación del comprador
        Completed = 6,                     // comprador confirmó o autocierre por tiempo con evidencia
        Disputed = 7,                      // comprador reportó problema
        Rejected = 8,                      // productor rechazó
        CancelledByUser = 9,               // cancelada por el comprador (antes de aceptación)
        Expired = 10                       // expiró por inacción (p.ej., productor no decidió)
    }
}
