namespace Entity.Domain.Enums
{
    public enum OrderStatus
    {
        PendingReview = 0,        // creada, con comprobante, esperando decisión del productor
        AcceptedAwaitingUser = 1, // aceptada por el productor, a la espera de confirmación del cliente
        Rejected = 2,             // rechazada por el productor
        Completed = 3,            // cliente confirmó recepción ("Yes")
        Disputed = 4              // cliente reporta no recibido ("No")
    }
}
