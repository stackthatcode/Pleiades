using System.Collections.Generic;

namespace Commerce.Application.Model.Orders
{
    public static class OrderLineStatusExtensions
    {
        private static new Dictionary<OrderLineStatus, string> _statusToDescription = new Dictionary<OrderLineStatus, string>()
            {
                { OrderLineStatus.Pending, "Pending" },
                { OrderLineStatus.Shipped, "Shipped" },
                { OrderLineStatus.Received, "Received" },
                { OrderLineStatus.ShippingFailure, "Shipping Failure" },
                { OrderLineStatus.Refunded, "Refunded" },
                { OrderLineStatus.Cancelled, "Cancelled" },
            };

        public static string ToDescription(this OrderLineStatus status)
        {
            return _statusToDescription[status];
        }
    }
}