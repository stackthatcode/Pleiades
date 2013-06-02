﻿using System.Collections.Generic;

namespace Commerce.Persist.Model.Orders
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