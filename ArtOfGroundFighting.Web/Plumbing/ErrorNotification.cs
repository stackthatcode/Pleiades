using System;
using Autofac;
using Autofac.Integration.Mvc;
using Commerce.Application.Email;
using Pleiades.App.Logging;
using Pleiades.Web.Activity;

namespace ArtOfGroundFighting.Web.Plumbing
{
    public class ErrorNotification
    {
        public static void Send(Exception ex)
        {
            try
            {
                using (var scope = AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope())
                {
                    var activityId = ActivityId.Current;
                    var builder = scope.Resolve<IAdminEmailBuilder>();
                    var message = builder.SystemError(activityId, ex);
                    var service = scope.Resolve<IEmailService>();
                    service.Send(message);
                }
            }
            catch (Exception innerException)
            {
                LoggerSingleton.Get().Error(innerException);
            }
        }
    }
}