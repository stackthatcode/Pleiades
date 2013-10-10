namespace Commerce.Application.Model.Orders
{
    public enum OrderLineStatus
    {
        Pending = 1,
        Shipped = 2,
        Received = 3,
        ShippingFailure = 4,
        Refunded = 5,
        Cancelled = 6,
    }
}
