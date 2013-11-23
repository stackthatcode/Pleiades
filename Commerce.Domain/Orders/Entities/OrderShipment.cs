using System.Collections.Generic;

namespace Commerce.Application.Orders.Entities
{
    public class OrderShipment
    {
        public Order Order { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public List<OrderLineGroup> OrderLineGroups
        {
            get { return OrderLines.ToOrderLineGroups(); }
        } 
    }
}
