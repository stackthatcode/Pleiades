using System;

namespace Commerce.Application.Model.Email
{
    public class Message
    {
        public Message()
        {
            DateCreated = DateTime.Now;
        }

        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
