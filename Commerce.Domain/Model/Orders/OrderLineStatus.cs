namespace Commerce.Persist.Model.Orders
{
    public enum OrderLineStatus
    {
        Pending = 1,
        Shipped = 2,
        ShippingFail = 3,
        Received = 4,
        Refunded = 5,
        Cancelled = 6,
    }
}
