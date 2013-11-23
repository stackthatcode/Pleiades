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
            output.Subject = "Your order has been received";
            output.Body = _templateEngine.Render(order, TemplateIdentifier.CustomerOrderReceived);
            return output;
        }

        public EmailMessage OrderItemsShipped(OrderShipment shipment)
        {
            var output = MessageFactory();
            output.To = shipment.Order.EmailAddress;
            output.Subject = "Items in your order have shipped";
            output.Body = _templateEngine.Render(shipment, TemplateIdentifier.CustomerOrderItemsShipped);
            return output;
        }

        public EmailMessage OrderItemsRefunded(OrderRefund refund)
        {
            var output = MessageFactory();
            output.To = refund.Order.EmailAddress;
            output.Subject = "Items in your order have been refunded";
            output.Body = _templateEngine.Render(refund, TemplateIdentifier.CustomerOrderReceived);
            return output;
        }
    }
}
