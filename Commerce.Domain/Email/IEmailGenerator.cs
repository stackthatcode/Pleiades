using System.Collections.Generic;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Email
{
    //
    // TODO: where do the Razor Templates live...?  In Commerce.Web?
    //
    public interface IEmailGenerator
    {
        EmailMessage OrderReceived(Order order);
        EmailMessage OrderItemsShipped(Order order, List<OrderLine> orderLine);
        EmailMessage OrderItemsRefunded(Order order, List<OrderLine> orderLine);
        EmailMessage SystemError(string synopsis);
    }
}
