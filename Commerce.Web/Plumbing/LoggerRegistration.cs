using System.Configuration;
using Pleiades.App.Logging;
using Pleiades.App.Utility;
using Pleiades.Web.Activity;
using Commerce.Application.Azure;


namespace Commerce.Web.Plumbing
{
    public class LoggerRegistration
    {
        public static void Register()
        {
            var loggerName = (ConfigurationManager.AppSettings["StorefrontName"] ?? "Commerce") + ".Admin";

            if (ConfigurationManager.AppSettings["AzureHosted"].ToBoolTryParse())
            {
                LoggerSingleton.Get = AzureLogger.RegistrationFactory(loggerName, ActivityId.MessageFormatter);
            }
            else
            {
                LoggerSingleton.Get = NLoggerImpl.RegistrationFactory(loggerName, ActivityId.MessageFormatter);
            }
        }
    }
}
