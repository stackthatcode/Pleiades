using System;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Email;

namespace Commerce.Persist.Concrete
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
