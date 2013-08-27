using Commerce.Application.Interfaces;
using Commerce.Application.Model.Email;

namespace Commerce.Application.Concrete.Infrastructure
{
    // TODO: store Company Name in configuration?  NameValueCollection?
    // TODO: add Context specific arguments?
    // TODO: add Embedded Content, what-not-ish
    // TODO: Get down with this: http://martinnormark.com/generate-html-e-mail-body-in-c-using-templates

    public class EmailGenerator : IEmailGenerator
    {
        public Message OrderReceived()
        {
            return new Message()
            {
                 Subject = "We have received your order"
            };
        }

        public Message OrderShipped()
        {
            return new Message()
            {
                Subject = "Your order has shipped"
            };
        }
    }
}
