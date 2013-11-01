using System;
using System.Collections.Generic;
using Commerce.Application.Model.Analytics;

namespace Commerce.Application.Interfaces
{
    public interface IAnalyticsRepository
    {
        List<PurchaseOrderEvent> PurchaseOrderEventsByDate(DateTime from, DateTime to);
        List<PurchaseSkuEvent> PurchaseSkuEventsByDate(DateTime from, DateTime to);
        List<RefundSkuEvent> RefundSkuEventsByDate(DateTime from, DateTime to);
        List<RefundEvent> RefundEventsByDate(DateTime from, DateTime to);
    }
}
