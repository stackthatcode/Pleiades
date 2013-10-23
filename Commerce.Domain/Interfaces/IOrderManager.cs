using System;
using System.Collections.Generic;
using Commerce.Application.Model.Orders;

namespace Commerce.Application.Interfaces
{
    public interface IOrderManager
    {
        List<Order> Find(DateTime? startDate, DateTime? endDate, bool? complete);
        Order Retrieve(string externalId);
        void Refund(string externalId);
        void Refund(string externalId, List<OrderLineChange> itemsAndQuantities);
        void Ship(string externalId);
        void Ship(string externalId, List<OrderLineChange> itemsAndQuantities);
        void Cancel(string externalId);
        void Cancel(string externalId, List<OrderLineChange> itemsAndQuantities);
    }
}
