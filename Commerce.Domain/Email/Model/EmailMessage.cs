using System;

namespace Commerce.Application.Email.Model
{
    public class EmailMessage
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateSent { get; set; }

        public EmailMessage()
        {
            DateSent = DateTime.Now;
        }
    }
}
