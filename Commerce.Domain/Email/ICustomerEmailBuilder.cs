using System.Collections.Generic;
using Commerce.Application.Email.Model;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Email
{
    public interface ICustomerEmailBuilder
    {
        EmailMessage OrderReceived(Order order);
        EmailMessage OrderItemsShipped(OrderShipment shipment);
        EmailMessage OrderItemsRefunded(OrderRefund refund);
    }
}
