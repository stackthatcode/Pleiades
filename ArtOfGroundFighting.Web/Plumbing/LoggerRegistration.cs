using System.Configuration;
using Commerce.Application.Azure;
using Pleiades.App.Logging;
using Pleiades.App.Utility;
using Pleiades.Web.Activity;

namespace ArtOfGroundFighting.Web.Plumbing
{
    public class LoggerRegistration
    {
        public static void Register()
        {
            var loggerName = (ConfigurationManager.AppSettings["StorefrontName"] ?? "ArtOfGroundFighting") + ".Storefront";

            LoggerSingleton.Get = 
                ConfigurationManager.AppSettings["AzureHosted"].ToBoolTryParse() 
                    ? AzureLogger.RegistrationFactory(loggerName, ActivityId.MessageFormatter) 
                    : NLoggerImpl.RegistrationFactory(loggerName, ActivityId.MessageFormatter);
        }
    }
}
