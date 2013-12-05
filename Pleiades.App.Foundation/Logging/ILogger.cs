using System;

namespace Pleiades.App.Logging
{
    public interface ILogger
    {
        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(Exception exception);
        void Fatal(string message);
    }
}
