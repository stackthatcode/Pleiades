using System;
using Autofac;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Composites;
using Pleiades.Web.Security.Execution.Step;
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

            // Aspect
            builder.RegisterType<SystemAuthorizeAttribute>().InstancePerLifetimeScope();

            // Services
            builder.RegisterType<AggregateUserService>().As<IAggregateUserService>().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextUserService>().As<IHttpContextUserService>().InstancePerLifetimeScope();
            builder.RegisterType<OwnerAuthorizationService>().As<IOwnerAuthorizationService>().InstancePerLifetimeScope();

            // Authorization Steps
            builder.RegisterType<AccountLevelAuthorizationStep>().InstancePerLifetimeScope();
            builder.RegisterType<AccountStatusAuthorizationStep>().InstancePerLifetimeScope();
            builder.RegisterType<RoleAuthorizationStep>().InstancePerLifetimeScope();
            builder.RegisterType<GetUserFromHttpContextStep>().InstancePerLifetimeScope();
            builder.RegisterType<SystemAuthorizationComposite>().InstancePerLifetimeScope();            
        }   
    }
}