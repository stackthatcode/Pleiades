using System;
using Commerce.Application.Email.Model;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Email
{
    public interface IAdminEmailBuilder
    {
        EmailMessage OrderReceived(Order order);
        EmailMessage OrderItemsShipped(OrderShipment shipment);
        EmailMessage OrderItemsRefunded(OrderRefund refund);
        EmailMessage SystemError(Guid activityId, Exception exception);
    }
}
