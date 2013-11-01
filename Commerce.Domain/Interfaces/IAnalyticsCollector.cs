using System;
using System.Collections.Generic;
using Commerce.Application.Model.Analytics;
using Commerce.Application.Model.Orders;

namespace Commerce.Application.Interfaces
{
    public interface IAnalyticsCollector
    {
        void Sale(Order order);
        void Refund(DateTime date, int orderId, List<RefundItem> items);
    }
}
