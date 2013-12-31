using System;
using System.IO;
using Commerce.Application.Email.Model;

namespace Commerce.Application.Email
{
    public class MockEmailService : IEmailService
    {
        private readonly IEmailConfigAdapter _configAdapter;
        private readonly TimeSpan _emailLifespan = new TimeSpan(-1, 0, 0, 0);

        public MockEmailService(IEmailConfigAdapter configAdapter)
        {
            _configAdapter = configAdapter;
            
            CleanUpMailDump();
        }

        public void CleanUpMailDump()
        {            
            var directoryInfo = new DirectoryInfo(_configAdapter.MockServiceOutputDirectory);
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(_configAdapter.MockServiceOutputDirectory);
            }
            else
            {
                foreach (var fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    if (fileInfo.CreationTime < DateTime.Now.Add(_emailLifespan))
                    {
                        fileInfo.Delete();
                    }
                }
            }
        }

        public void Send(EmailMessage emailMessage)
        {
            var timestamp = DateTime.Now;
            var contents =
                "Date: " + DateTime.Now + Environment.NewLine +
                "To: " + emailMessage.To + Environment.NewLine +
                "From: " + emailMessage.From + Environment.NewLine + 
                "Subject: " + emailMessage.Subject + Environment.NewLine + Environment.NewLine + 
                emailMessage.Body;

            var fileName = 
                timestamp.Year + 
                timestamp.Month.ToString("00") + 
                timestamp.Day.ToString("00") + "_" + 
                timestamp.Hour.ToString("00") + "." + 
                timestamp.Minute.ToString("00") + "." + 
                timestamp.Second.ToString("00") + "." + 
                timestamp.Millisecond.ToString("000") + 
                Guid.NewGuid().ToString().Substring(0, 4) + "_" + 
                emailMessage.To + ".txt";

            var filePath = Path.Combine(_configAdapter.MockServiceOutputDirectory, fileName);
            System.IO.File.WriteAllText(filePath, contents);
        }
    }
}
