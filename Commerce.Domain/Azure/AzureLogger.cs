using System;
using Pleiades.App.Logging;

namespace Commerce.Application.Azure
{
    public class AzureLogger : ILogger
    {
        // Storage Account: Pushmarket => Storage Container: ArtOfGroundFighting => Table Name: ApplicationLogs

        private readonly AzureLogEntryRepository _repository;
        private readonly string _loggerName;
        private AzureLogLevel _azureLogLevel = AzureLogLevel.Error;
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
            _azureLogLevel = AzureConfiguration.Settings.LogLevel;
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
            if (_azureLogLevel <= AzureLogLevel.Trace)
            {
                ExceptionEater(() => _repository.AddEntry(_loggerName, "Trace", _messageFormatter(message)));
            }
        }

        public void Debug(string message)
        {
            if (_azureLogLevel <= AzureLogLevel.Debug)
            {
                ExceptionEater(() => _repository.AddEntry(_loggerName, "Debug", _messageFormatter(message)));
            }
        }

        public void Info(string message)
        {
            if (_azureLogLevel <= AzureLogLevel.Info)
            {
                ExceptionEater(() => _repository.AddEntry(_loggerName, "Info", _messageFormatter(message)));
            }
        }

        public void Warn(string message)
        {
            if (_azureLogLevel <= AzureLogLevel.Warn)
            {
                ExceptionEater(() => _repository.AddEntry(_loggerName, "Warn", _messageFormatter(message)));
            }
        }

        public void Error(string message)
        {
            if (_azureLogLevel <= AzureLogLevel.Error)
            {
                ExceptionEater(() => _repository.AddEntry(_loggerName, "Error", _messageFormatter(message)));
            }
        }

        public void Error(Exception exception)
        {
            if (_azureLogLevel <= AzureLogLevel.Error)
            {
                ExceptionEater(
                    () => _repository.AddEntry(
                        _loggerName, "Error", _messageFormatter(exception.FullStackTraceDump())));
            }
        }

        public void Fatal(string message)
        {
            if (_azureLogLevel <= AzureLogLevel.Critical)
            {
                ExceptionEater(() => _repository.AddEntry(_loggerName, "Fatal", _messageFormatter(message)));
            }
        }
    }
}
