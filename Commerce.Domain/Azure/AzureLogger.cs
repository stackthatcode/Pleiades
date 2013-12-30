using System;
using Pleiades.App.Logging;

namespace Commerce.Application.Azure
{
    public class AzureLogger : ILogger
    {
        private readonly AzureLogEntryRepository _repository;
        private readonly string _loggerName;
        private readonly Func<string, string> _messageFormatter = x => x;

        public static Func<ILogger> RegistrationFactory(string loggerName, Func<string, string> formatter = null)
        {
            ILogger logger = new AzureLogger(loggerName, formatter);
            return () => logger;
        }

        public AzureLogger(string loggerName, Func<string, string> formatter)
        {            
            _loggerName = loggerName;
            _repository = new AzureLogEntryRepository(AzureConfiguration.Settings);            
            if (formatter != null)
            {
                _messageFormatter = formatter;
            }
        }

        public void Trace(string message)
        {
            _repository.AddEntry(_loggerName, "Trace", _messageFormatter(message));
        }

        public void Debug(string message)
        {
            _repository.AddEntry(_loggerName, "Debug", _messageFormatter(message));
        }

        public void Info(string message)
        {
            _repository.AddEntry(_loggerName, "Info", _messageFormatter(message));
        }

        public void Warn(string message)
        {
            _repository.AddEntry(_loggerName, "Warn", _messageFormatter(message));
        }

        public void Error(string message)
        {
            _repository.AddEntry(_loggerName, "Error", _messageFormatter(message));
        }

        public void Error(Exception exception)
        {
            _repository.AddEntry(_loggerName, "Error", _messageFormatter(exception.FullStackTraceDump()));
        }

        public void Fatal(string message)
        {
            _repository.AddEntry(_loggerName, "Fatal", _messageFormatter(message));
        }
    }
}
