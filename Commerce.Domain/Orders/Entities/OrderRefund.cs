using System.Collections.Generic;
using Commerce.Application.Billing;

namespace Commerce.Application.Orders.Entities
{
    public class OrderRefund
    {
        public Order Order { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public List<OrderLineGroup> OrderLineGroups
        {
            get { return OrderLines.ToOrderLineGroups(); }
        }
        public Transaction Transaction { get; set; }
    }
}
