using System;
using Autofac;
using Autofac.Integration.Mvc;
using Commerce.Application.Email;
using Pleiades.Application.Logging;
using Pleiades.Web.Logging;

namespace Commerce.Web.Plumbing
{
    public class ErrorNotification
    {
        public static void Send(Exception ex)
        {
            try
            {
                var activityId = ActivityId.Current;
                var container = AutofacDependencyResolver.Current.ApplicationContainer;
                var builder = container.Resolve<IAdminEmailBuilder>();
                var message = builder.SystemError(activityId, ex);
                var service = container.Resolve<IEmailService>();
                service.Send(message);
            }
            catch (Exception innerException)
            {
                LoggerSingleton.Get().Error(innerException);
            }
        }
    }
}