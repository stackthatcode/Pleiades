using System.Collections.Generic;
using Commerce.Application.Email.Model;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Email
{
    public interface ICustomerEmailBuilder
    {
        EmailMessage OrderReceived(Order order);
        EmailMessage OrderItemsShipped(Order order, List<OrderLine> orderLine);
        EmailMessage OrderItemsRefunded(Order order, List<OrderLine> orderLine);
    }
}
