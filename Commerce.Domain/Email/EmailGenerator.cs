using System.Collections.Generic;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Email
{
    // TODO: Get down with this: http://martinnormark.com/generate-html-e-mail-body-in-c-using-templates    
    public class EmailGenerator : IEmailGenerator
    {
        public EmailMessage OrderReceived()
        {
            return new EmailMessage()
            {
                 Subject = "We have received your order"
            };
        }

        public EmailMessage OrderShipped()
        {
            return new EmailMessage()
            {
                Subject = "Your order has shipped"
            };
        }

        public EmailMessage OrderReceived(Order order)
        {
            throw new System.NotImplementedException();
        }

        public EmailMessage OrderItemsShipped(Order order, List<OrderLine> orderLine)
        {
            throw new System.NotImplementedException();
        }

        public EmailMessage OrderItemsRefunded(Order order, List<OrderLine> orderLine)
        {
            throw new System.NotImplementedException();
        }

        public EmailMessage SystemError(string synopsis)
        {
            throw new System.NotImplementedException();
        }
    }
}
