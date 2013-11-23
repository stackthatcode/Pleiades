using System;
using System.Collections.Generic;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Orders
{
    public interface IOrderManager
    {
        List<Order> Find(DateTime? startDate, DateTime? endDate, bool? complete);
        Order Retrieve(string externalId);
        OrderRefund Refund(string externalId, List<int> items);
        OrderShipment Ship(string externalId, List<int> items);

        // DO WE NEED THESE...?
        void FailShipping(string externalId, List<int> items);
        void Return(string externalId, List<int> items);
    }
}
