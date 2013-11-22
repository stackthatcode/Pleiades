using System.Collections.Generic;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Email.Model
{
    public class OrderItemsShippedModel
    {
        public Order Order { get; set; }
        public List<OrderLineGroup> ShippedOrderLines { get; set; }
    }
}
