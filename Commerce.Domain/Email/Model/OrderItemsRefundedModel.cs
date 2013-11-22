using System.Collections.Generic;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Email.Model
{
    public class OrderItemsRefundedModel
    {
        public Order Order { get; set; }
        public List<OrderLineGroup> RefundedOrderLines { get; set; }
    }
}
