using System;
using NLog;
using Pleiades.Application.Logging;

namespace Pleiades.Web.Logging
{
    public class NLoggerImpl : ILogger
    {
        private readonly string _loggerName;

        public static Func<ILogger> RegistrationFactory(string loggerName, Func<string, string> formatter = null)
        {
            ILogger logger = new NLoggerImpl(loggerName, formatter);            
            return () => logger;
        }

        public Func<string, string> MessageFormatter = x => x;

        public NLoggerImpl(string loggerName, Func<string, string> formatter)
        {
            _loggerName = loggerName;
            if (formatter != null)
            {
                MessageFormatter = formatter;
            }
        }

        public Logger GetLogger
        {
            get { return LogManager.GetLogger(_loggerName); }
        }

        public void Trace(string message)
        {
            GetLogger.Trace(MessageFormatter(message));
        }

        public void Debug(string message)
        {
            GetLogger.Debug(MessageFormatter(message));
        }

        public void Info(string message)
        {
            GetLogger.Info(MessageFormatter(message));
        }

        public void Warn(string message)
        {
            GetLogger.Warn(MessageFormatter(message));
        }

        public void Error(string message)
        {
            GetLogger.Error(MessageFormatter(message));
        }

        public void Error(Exception exception)
        {
            GetLogger.Error(MessageFormatter(exception.FullStackTraceDump()));
        }

        public void Fatal(string message)
        {
            GetLogger.Fatal(MessageFormatter(message));
        }
    }
}
 