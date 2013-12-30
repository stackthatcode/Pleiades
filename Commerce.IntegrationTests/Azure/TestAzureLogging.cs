using Commerce.Application.Azure;
using NUnit.Framework;
using Pleiades.App.Logging;

namespace Commerce.IntegrationTests.Azure
{
    [TestFixture]
    public class TestAzureLogging
    {
        [Test]
        public void SendAnInfoMessage()
        {
            var loggerName = "TestApplication";
            LoggerSingleton.Get = AzureLogger.RegistrationFactory(loggerName);
            var logger = LoggerSingleton.Get();
            logger.Info("Test Message");
        }
    }
}
