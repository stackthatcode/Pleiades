using System;
using System.Collections.Generic;
using Commerce.Application.Model.Analytics;

namespace Commerce.Application.Interfaces
{
    public interface IAnalyticsAggregator
    {
        List<DateTotal> TotalSalesAmountsByDate(DateTime @from, DateTime to);
        List<DateTotal> TotalSalesQuantitiesByDate(DateTime @from, DateTime to);
        List<DateTotal> TotalRefundAmountsByDate(DateTime @from, DateTime to);
        List<DateTotal> TotalRefundQuantitiesByDate(DateTime @from, DateTime to);

        List<SkuDateTotal> TotalSalesAmountsByDateAndSku(DateTime @from, DateTime to);
        List<SkuDateTotal> TotalSalesQuantitiesByDateAndSku(DateTime @from, DateTime to);
        List<SkuDateTotal> TotalRefundAmountsByDateAndSku(DateTime @from, DateTime to);
        List<SkuDateTotal> TotalRefundQuantitiesByDateAndSku(DateTime @from, DateTime to);

        List<SkuTotal> TotalSalesAmountsBySku(DateTime @from, DateTime to);
        List<SkuTotal> TotalSalesQuantityBySku(DateTime @from, DateTime to);
        List<SkuTotal> TotalRefundAmountsBySku(DateTime @from, DateTime to);
        List<SkuTotal> TotalRefundQuantityBySku(DateTime @from, DateTime to); 
    }
}
