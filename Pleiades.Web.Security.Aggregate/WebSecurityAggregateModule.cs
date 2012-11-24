using System;
using Autofac;
using Pleiades.Injection;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Rules;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security
{
    public class WebSecurityAggregateModule : Module
    {       
        protected override void Load(ContainerBuilder builder)
        {
            // Pleiades.Web.Security.MembershipProvider            
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerLifetimeScope();

            // Concrete Services
            builder.RegisterType<AggregateUserService>().As<IAggregateUserService>().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextUserService>().As<IHttpContextUserService>().InstancePerLifetimeScope();
            builder.RegisterType<OwnerAuthorizationService>().As<IOwnerAuthorizationService>().InstancePerLifetimeScope();

            // Aspect
            builder.RegisterType<SecurityAttribute>().InstancePerLifetimeScope();
        }   
    }
}