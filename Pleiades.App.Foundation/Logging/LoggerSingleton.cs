using System;

namespace Pleiades.App.Logging
{
    public class LoggerSingleton
    {
        public static Func<ILogger> Get = () => new ConsoleAndDebugLogger();
    }
}
