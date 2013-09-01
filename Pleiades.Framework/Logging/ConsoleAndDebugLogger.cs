using System;

namespace Pleiades.Application.Logging
{
    public class ConsoleAndDebugLogger : ILogger
    {
        public void Trace(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Debug(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Warn(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Error(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Fatal(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
