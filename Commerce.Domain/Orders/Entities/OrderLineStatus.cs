namespace Commerce.Application.Orders.Entities
{
    public enum OrderLineStatus
    {
        Pending = 1,
        Shipped = 2,
        FailedShipping = 5,
        Returned = 3,
        Refunded = 4,
    }
}
