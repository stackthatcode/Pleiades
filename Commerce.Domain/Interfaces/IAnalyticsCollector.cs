using System.Collections.Generic;
using Commerce.Application.Model.Orders;

namespace Commerce.Application.Interfaces
{
    public interface IAnalyticsCollector
    {
        void Sale(Order order);
        void Refund(List<string> skuCodes, decimal refundAmount);
    }
}
