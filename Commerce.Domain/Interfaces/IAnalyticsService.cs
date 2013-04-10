using System;
using Commerce.Persist.Model.Orders;

namespace Commerce.Persist.Interfaces
{
    public interface IAnalyticsService
    {
        void AddSale(Order order);
    }
}