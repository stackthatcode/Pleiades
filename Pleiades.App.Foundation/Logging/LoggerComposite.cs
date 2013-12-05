using System;
using System.Collections.Generic;

namespace Pleiades.App.Logging
{
    public class LoggerComposite : ILogger
    {
        readonly List<ILogger> _loggers;

        public LoggerComposite()
        {
            _loggers = new List<ILogger>();
        }

        public LoggerComposite Add(ILogger logger)
        {
            _loggers.Add(logger);
            return this;
        }

        public void Trace(string message)
        {
            _loggers.ForEach(x => x.Trace(message));
        }

        public void Debug(string message)
        {
            _loggers.ForEach(x => x.Debug(message));
        }

        public void Info(string message)
        {
            _loggers.ForEach(x => x.Info(message));
        }

        public void Warn(string message)
        {
            _loggers.ForEach(x => x.Warn(message));
        }

        public void Error(string message)
        {
            _loggers.ForEach(x => x.Error(message));
        }

        public void Error(Exception exception)
        {
            _loggers.ForEach(x => x.Error(exception));
        }

        public void Fatal(string message)
        {
            _loggers.ForEach(x => x.Fatal(message));
        }
    }
}
