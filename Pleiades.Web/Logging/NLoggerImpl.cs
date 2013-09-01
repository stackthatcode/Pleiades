using System;
using NLog;
using Pleiades.Application.Logging;

namespace Pleiades.Web.Logging
{
    public class NLoggerImpl : ILogger
    {
        private readonly Logger _fileLogger;
        private readonly ConsoleAndDebugLogger _consoleAndDebugLogger;

        public static Func<ILogger> RegistrationFactory(bool consoleEnabled)
        {
            var logger = new NLoggerImpl(consoleEnabled);
            return () => logger;
        }

        public NLoggerImpl(bool consoleEnabled)
        {
            _fileLogger = LogManager.GetCurrentClassLogger();
            if (consoleEnabled)
                _consoleAndDebugLogger = new ConsoleAndDebugLogger();
        }

        public void Trace(string message)
        {
            _fileLogger.Trace(message);
            if (_consoleAndDebugLogger != null)
                _consoleAndDebugLogger.Trace(message);
        }

        public void Debug(string message)
        {
            _fileLogger.Debug(message);
            if (_consoleAndDebugLogger != null)
                _consoleAndDebugLogger.Trace(message);
        }

        public void Info(string message)
        {
            _fileLogger.Info(message);
            if (_consoleAndDebugLogger != null)
                _consoleAndDebugLogger.Trace(message);
        }

        public void Warn(string message)
        {
            _fileLogger.Warn(message);
            if (_consoleAndDebugLogger != null)
                _consoleAndDebugLogger.Trace(message);
        }

        public void Error(string message)
        {
            _fileLogger.Error(message);
            if (_consoleAndDebugLogger != null)
                _consoleAndDebugLogger.Error(message);
        }

        public void Fatal(string message)
        {
            _fileLogger.Fatal(message);
            if (_consoleAndDebugLogger != null)
                _consoleAndDebugLogger.Fatal(message);
        }
    }
}
 