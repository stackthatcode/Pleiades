using System.Collections.Generic;
using Commerce.Application.Email.Model;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Email
{
    public class CustomerEmailBuilder : ICustomerEmailBuilder
    {
        private readonly IEmailConfigAdapter _config;
        private readonly ITemplateEngine _templateEngine;

        public CustomerEmailBuilder(
                IEmailConfigAdapter config, 
                ITemplateEngine templateEngine)
        {
            _config = config;
            _templateEngine = templateEngine;
        }

        private EmailMessage MessageFactory()
        {
            return new EmailMessage
            {
                From = _config.CustomerServiceEmail,
            };
        }

        public EmailMessage OrderReceived(Order order)
        {
            var output = MessageFactory();
            output.To = order.EmailAddress;
            output.Body = _templateEngine.Render(
                order, "Commerce.Application.Email.Templates.Customer.OrderReceived.txt");
            return output;
        }

        public EmailMessage OrderItemsShipped(Order order, List<OrderLine> orderLine)
        {
            var output = MessageFactory();
            output.To = order.EmailAddress;
            output.Body = _templateEngine.Render(
                order, "Commerce.Application.Email.Templates.Customer.OrderReceived.txt");
            return output;
        }

        public EmailMessage OrderItemsRefunded(Order order, List<OrderLine> orderLine)
        {
            var output = MessageFactory();
            output.To = order.EmailAddress;
            output.Body = _templateEngine.Render(
                order, "Commerce.Application.Email.Templates.Customer.OrderReceived.txt");
            return output;
        }
    }
}
