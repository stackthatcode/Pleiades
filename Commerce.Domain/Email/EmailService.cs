using System;
using Commerce.Application.Email.Model;
using System.Net.Mail;
using System.Net;
using Pleiades.App.Logging;

namespace Commerce.Application.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfigAdapter _emailConfigAdapter;

        public EmailService(IEmailConfigAdapter emailConfigAdapter)
        {
            _emailConfigAdapter = emailConfigAdapter;
        }

        public void Send(EmailMessage emailMessage)
        {
            try
            {
                var client = new SmtpClient(_emailConfigAdapter.SmtpHost, Int32.Parse(_emailConfigAdapter.SmtpPort))
                {
                    UseDefaultCredentials = false
                };
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(
                    _emailConfigAdapter.SmtpUserName, _emailConfigAdapter.SmtpPassword);

                var message = new MailMessage(emailMessage.From, emailMessage.To);
                message.Subject = emailMessage.Subject;
                message.Body = emailMessage.Body;
                client.Send(message); 
            }
            catch (Exception ex)
            {
                LoggerSingleton.Get().Error(ex);
            }
        }
    }
}
