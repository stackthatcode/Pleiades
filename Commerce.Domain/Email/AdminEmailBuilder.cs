using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Email.Model;
using Commerce.Application.Orders.Entities;
using Pleiades.App.Logging;
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

        public const string SubjectPrefix = "Admin Notification: ";

        public EmailMessage OrderReceived(Order order)
        {
            var output = MessageFactory();
            output.Subject = SubjectPrefix + "Order "+ order.ExternalId + " has been received";
            output.Body = _templateEngine.Render(order, TemplateIdentifier.AdminOrderReceived, false);
            return output;
        }

        public EmailMessage OrderItemsShipped(OrderShipment shipment)
        {
            var output = MessageFactory();
            output.Subject = SubjectPrefix + "Order " + shipment.Order.ExternalId + " has items shipped";
            output.Body = _templateEngine.Render(shipment, TemplateIdentifier.AdminOrderItemsShipped, false);
            return output;
        }

        public EmailMessage OrderItemsRefunded(OrderRefund refund)
        {
            var output = MessageFactory();
            output.Subject = SubjectPrefix + "Order " + refund.Order.ExternalId + " has items refunded";
            output.Body = _templateEngine.Render(refund, TemplateIdentifier.AdminOrderItemsRefunded, false);
            return output;
        }

        public EmailMessage SystemError(Guid activityId, Exception exception)
        {
            var output = MessageFactory();
            var subject = SubjectPrefix + "A System Error has occurred - " + activityId.ToString();
            output.Subject = subject;
            var body = subject + Environment.NewLine + Environment.NewLine + exception.FullStackTraceDump();
            output.Body = _templateEngine.Render(body, TemplateIdentifier.AdminSystemError, false);
            return output;
        }

        public EmailMessage CustomerContact(CustomerInquiry customerInquiry)
        {
            var output = MessageFactory();
            output.Subject = SubjectPrefix + "Customer Inquiry";
            output.Body = _templateEngine.Render(customerInquiry, TemplateIdentifier.AdminCustomerInquiry, false);
            return output;
        }
    }
}
