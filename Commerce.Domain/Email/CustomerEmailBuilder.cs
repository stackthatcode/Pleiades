using System;
using System.Collections.Generic;
using Commerce.Application.Email.Model;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Email
{
    public class CustomerEmailBuilder : ICustomerEmailBuilder
    {
        private readonly IEmailConfigAdapter _config;

        public CustomerEmailBuilder(IEmailConfigAdapter config)
        {
            _config = config;
        }

        private EmailMessage MessageFactory()
        {
            return new EmailMessage
            {
                From = _config.FromAddress,
            };
        }

        public EmailMessage OrderReceived(Order order)
        {
            var output = MessageFactory();
            output.To = order.EmailAddress;
            return output;
        }

        public EmailMessage OrderItemsShipped(Order order, List<OrderLine> orderLine)
        {
            var output = MessageFactory();
            output.To = order.EmailAddress;
            return output;
        }

        public EmailMessage OrderItemsRefunded(Order order, List<OrderLine> orderLine)
        {
            var output = MessageFactory();
            output.To = order.EmailAddress;
            return output;
        }
    }
}
