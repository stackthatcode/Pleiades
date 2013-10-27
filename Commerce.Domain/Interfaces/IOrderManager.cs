using System;
using System.Collections.Generic;
using Commerce.Application.Model.Orders;

namespace Commerce.Application.Interfaces
{
    public interface IOrderManager
    {
        List<Order> Find(DateTime? startDate, DateTime? endDate, bool? complete);
        Order Retrieve(string externalId);
        void RefundWithContextControl(string externalId, List<int> items);
        void Ship(string externalId, List<int> items);
        void FailShipping(string externalId, List<int> items);
        void Return(string externalId, List<int> items);
    }
}
