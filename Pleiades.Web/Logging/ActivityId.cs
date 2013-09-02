using System;
using System.Web;

namespace Pleiades.Web.Logging
{
    public class ActivityId
    {
        public const string ActivityIdKey = "ActivityIdKey";

        public static Guid Current
        {
            get
            {
                if (HttpContext.Current.Items[ActivityIdKey] == null)
                {
                    HttpContext.Current.Items[ActivityIdKey] = Guid.NewGuid();
                }
                return Guid.Parse(HttpContext.Current.Items[ActivityIdKey].ToString());
            }
        }

        public static string MessageFormatter(string message)
        {
            return ActivityId.Current.ToString() + "|" + message ?? "";
        }
    }
}
