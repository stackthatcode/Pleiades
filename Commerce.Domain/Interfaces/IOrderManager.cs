using System;
using System.Collections.Generic;
using Commerce.Application.Model.Orders;

namespace Commerce.Application.Interfaces
{
    public interface IOrderManager
    {
        List<Order> Find(DateTime? startDate, DateTime? endDate, bool? complete);
        Order Retrieve(string externalId);
        Order Refund(string externalId, List<int> items);
        Order Ship(string externalId, List<int> items);

        // DO WE NEED THESE...?
        void FailShipping(string externalId, List<int> items);
        void Return(string externalId, List<int> items);
    }
}
