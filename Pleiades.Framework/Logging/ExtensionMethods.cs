using System;

namespace Pleiades.Application.Logging
{
    public static class ExtensionMethods
    {
        public static string FullStackTraceDump(this Exception exception)
        {
            var output =
                exception.GetType() + ":" + exception.Message + Environment.NewLine + exception.StackTrace;
            
            if (exception.InnerException != null)
            {
                output = output + 
                    Environment.NewLine + Environment.NewLine +
                    "INNER EXCEPTION" + Environment.NewLine + 
                    exception.InnerException.FullStackTraceDump();
            }
            return output;
        }
    }
}
