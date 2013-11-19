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

        public AdminEmailBuilder(
                IEmailConfigAdapter config, 
                IReadOnlyAggregateUserRepository repository)
        {
            _config = config;
            _repository = repository;
        }

        private EmailMessage MessageFactory()
        {
            var emailAddresses =
                _repository
                    .Retreive(new List<UserRole>() {UserRole.Admin})
                    .Select(x => x.Membership.Email);
            var to = string.Join(",", emailAddresses);
            return new EmailMessage
                {
                    To = to,
                    From = _config.FromAddress,
                };
        }

        public EmailMessage OrderReceived(Order order)
        {
            var output = MessageFactory();
            return output;
        }

        public EmailMessage OrderItemsShipped(Order order, List<OrderLine> orderLine)
        {
            var output = MessageFactory();
            return output;
        }

        public EmailMessage OrderItemsRefunded(Order order, List<OrderLine> orderLine)
        {
            var output = MessageFactory();
            return output;
        }

        public EmailMessage SystemError(string synopsis)
        {
            var output = MessageFactory();
            return output;
        }
    }
}
