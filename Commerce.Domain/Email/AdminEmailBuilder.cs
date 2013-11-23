using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Email.Model;
using Commerce.Application.Orders.Entities;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Application.Email
{
    public class AdminEmailBuilder : IAdminEmailBuilder
    {
        private readonly IEmailConfigAdapter _config;
        private readonly IReadOnlyAggregateUserRepository _repository;
        private readonly ITemplateEngine _templateEngine;

        public AdminEmailBuilder(
                IEmailConfigAdapter config, 
                IReadOnlyAggregateUserRepository repository, 
                ITemplateEngine templateEngine)
        {
            _config = config;
            _repository = repository;
            _templateEngine = templateEngine;
        }

        private EmailMessage MessageFactory()
        {
            var emailAddresses =
                _repository
                    .Retreive(new List<UserRole> { UserRole.Root, UserRole.Admin })
                    .Select(x => x.Membership.Email);
            var to = string.Join(",", emailAddresses);
            return new EmailMessage
                {
                    To = to,
                    From = _config.SystemEmail,
                };
        }

        public EmailMessage OrderReceived(Order order)
        {
            var output = MessageFactory();
            output.Subject = "Order "+ order.ExternalId + " has been received";
            output.Body = _templateEngine.Render(order, TemplateIdentifier.AdminOrderReceived);
            return output;
        }

        public EmailMessage OrderItemsShipped(OrderShipment shipment)
        {
            var output = MessageFactory();
            output.Subject = "Order " + shipment.Order.ExternalId + " has items shipped";
            output.Body = _templateEngine.Render(shipment, TemplateIdentifier.AdminOrderItemsShipped);
            return output;
        }

        public EmailMessage OrderItemsRefunded(OrderRefund refund)
        {
            var output = MessageFactory();
            output.Subject = "Order " + refund.Order.ExternalId + " has items refunded";
            output.Body = _templateEngine.Render(refund, TemplateIdentifier.AdminOrderItemsRefunded);
            return output;
        }

        public EmailMessage SystemError(string synopsis)
        {
            var output = MessageFactory();
            output.Subject = "A System Error has occurred";
            output.Body = _templateEngine.Render(synopsis, TemplateIdentifier.AdminSystemError);
            return output;
        }
    }
}
