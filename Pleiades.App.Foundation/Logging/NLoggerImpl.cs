using System;
using NLog;

namespace Pleiades.App.Logging
{
    public class NLoggerImpl : ILogger
    {
        private readonly string _loggerName;
        private readonly Func<string, string> _messageFormatter = x => x;

        public static Func<ILogger> RegistrationFactory(string loggerName, Func<string, string> formatter = null)
        {
            ILogger logger = new NLoggerImpl(loggerName, formatter);            
            return () => logger;
        }

        public NLoggerImpl(string loggerName, Func<string, string> formatter)
        {
            _loggerName = loggerName;
            if (formatter != null)
            {
                _messageFormatter = formatter;
            }
        }

        public Logger GetLogger
        {
            get { return LogManager.GetLogger(_loggerName); }
        }

        public void Trace(string message)
        {
            GetLogger.Trace(_messageFormatter(message));
        }

        public void Debug(string message)
        {
            GetLogger.Debug(_messageFormatter(message));
        }

        public void Info(string message)
        {
            GetLogger.Info(_messageFormatter(message));
        }

        public void Warn(string message)
        {
            GetLogger.Warn(_messageFormatter(message));
        }

        public void Error(string message)
        {
            GetLogger.Error(_messageFormatter(message));
        }

        public void Error(Exception exception)
        {
            GetLogger.Error(_messageFormatter(exception.FullStackTraceDump()));
        }

        public void Fatal(string message)
        {
            GetLogger.Fatal(_messageFormatter(message));
        }
    }
}
 