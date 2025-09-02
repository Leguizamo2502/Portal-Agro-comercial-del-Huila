namespace Entity.Domain.Enums
{
    public enum OrderStatus
    {
        PendingReview = 0,
        AcceptedAwaitingUser = 1,
        Rejected = 2,
        Completed = 3,
        Disputed = 4
    }
}
