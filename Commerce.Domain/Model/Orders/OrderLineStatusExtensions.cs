using System.Collections.Generic;

namespace Commerce.Application.Model.Orders
{
    public static class OrderLineStatusExtensions
    {
        private static readonly Dictionary<OrderLineStatus, string> StatusToDescription = 
                new Dictionary<OrderLineStatus, string>()
                {
                    { OrderLineStatus.Pending, "Pending" },
                    { OrderLineStatus.Shipped, "Shipped" },
                    { OrderLineStatus.FailedShipping, "Failed Shipping" },
                    { OrderLineStatus.Returned, "Returned" },
                    { OrderLineStatus.Refunded, "Refunded" },
                };

        public static string ToDescription(this OrderLineStatus status)
        {
            return StatusToDescription[status];
        }
    }
}