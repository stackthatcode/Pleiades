using System;
using Autofac.Integration.Mvc;
using Pleiades.Web.Logging;

namespace Commerce.Web.Plumbing
{
    public class ErrorEmailNotification
    {
        public static void Send(Exception ex)
        {
            var activityId = ActivityId.Current;
            var container = AutofacDependencyResolver.Current.ApplicationContainer;

            
        }
    }
}