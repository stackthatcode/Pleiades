using System;
using System.IO;

namespace Commerce.Application.Email
{
    public class MockEmailService : IEmailService
    {
        private readonly string _outputDirectory;
        private readonly IMessageRenderingEngine _messageRenderingEngine;

        public MockEmailService(string outputDirectory, IMessageRenderingEngine messageRenderingEngine)
        {
            _outputDirectory = outputDirectory;
            _messageRenderingEngine = messageRenderingEngine;
        }

        public void Send(EmailMessage emailMessage)
        {
            var timestamp = DateTime.Now;
            var contents = 
                "Date: " + DateTime.Now + Environment.NewLine +
                "To: " + emailMessage.To + Environment.NewLine +
                "From: " + Environment.NewLine + Environment.NewLine + 
                _messageRenderingEngine.Generate(emailMessage);

            var fileName = 
                timestamp.Year + timestamp.Month.ToString("00") + timestamp.Day.ToString("00") + 
                "_" + emailMessage.To + ".txt";

            var filePath = Path.Combine(_outputDirectory, fileName);
            System.IO.File.WriteAllText(filePath, contents);
        }
    }
}
