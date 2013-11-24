using System;
using Pleiades.Application.Logging;

namespace Pleiades.Web.Logging
{
    public class InsulatedExecution
    {
        public static void Act(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                LoggerSingleton.Get().Error(ex);
            }
        }
    }
}
