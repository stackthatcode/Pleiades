using System;
using Pleiades.App.Logging;

namespace Commerce.Application.Azure
{
    public class AzureLogger : ILogger
    {
        // Storage Account: Pushmarket => Storage Container: ArtOfGroundFighting => Table Name: ApplicationLogs

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

        private void ExceptionEater(Action doSomething)
        {
            try
            {
                doSomething();
            }
            catch (Exception ex)
            {               
                Console.WriteLine(ex.FullStackTraceDump());
                System.Diagnostics.Debug.WriteLine(ex.FullStackTraceDump());
            }
        }

        public void Trace(string message)
        {
            ExceptionEater(() => _repository.AddEntry(_loggerName, "Trace", _messageFormatter(message)));
        }

        public void Debug(string message)
        {
            ExceptionEater(() => _repository.AddEntry(_loggerName, "Debug", _messageFormatter(message)));
        }

        public void Info(string message)
        {
            ExceptionEater(() => _repository.AddEntry(_loggerName, "Info", _messageFormatter(message)));
        }

        public void Warn(string message)
        {
            ExceptionEater(() => _repository.AddEntry(_loggerName, "Warn", _messageFormatter(message)));
        }

        public void Error(string message)
        {
            ExceptionEater(() => _repository.AddEntry(_loggerName, "Error", _messageFormatter(message)));
        }

        public void Error(Exception exception)
        {
            ExceptionEater(
                () => _repository.AddEntry(
                    _loggerName, "Error", _messageFormatter(exception.FullStackTraceDump())));
        }

        public void Fatal(string message)
        {
            ExceptionEater(() => _repository.AddEntry(_loggerName, "Fatal", _messageFormatter(message)));
        }
    }
}
