using System;
using System.Collections.Generic;
using Commerce.Application.Analytics.Entities;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Analytics
{
    public interface IAnalyticsCollector
    {
        void Sale(Order order);
        void Refund(DateTime date, int orderId, List<RefundItem> items);
    }
}
