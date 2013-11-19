using System;
using System.IO;
using Commerce.Application.Email.Model;

namespace Commerce.Application.Email
{
    public class MockEmailService : IEmailService
    {
        private readonly IEmailConfigAdapter _configAdapter;

        public MockEmailService(IEmailConfigAdapter configAdapter)
        {
            _configAdapter = configAdapter;
        }

        public void Send(EmailMessage emailMessage)
        {
            var timestamp = DateTime.Now;
            var contents =
                "Date: " + DateTime.Now + Environment.NewLine +
                "To: " + emailMessage.To + Environment.NewLine +
                "From: " + Environment.NewLine + Environment.NewLine +
                emailMessage.Body;

            var fileName = 
                timestamp.Year + timestamp.Month.ToString("00") + timestamp.Day.ToString("00") + 
                "_" + emailMessage.To + ".txt";

            var filePath = Path.Combine(_configAdapter.MockServiceOutputDirectory, fileName);
            System.IO.File.WriteAllText(filePath, contents);
        }
    }
}
