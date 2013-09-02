using System;

namespace Pleiades.Application.Logging
{
    public class LoggerSingleton
    {
        public static Func<ILogger> Get = () => new ConsoleAndDebugLogger();
    }
}
