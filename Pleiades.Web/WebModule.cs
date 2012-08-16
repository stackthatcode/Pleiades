using System;
using Autofac;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Concrete;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Framework.MembershipProvider.Concrete;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.Web.Security.Aspect;
using Pleiades.Framework.Web.Security.Concrete;
using Pleiades.Framework.Web.Security.Execution.Abstract;
using Pleiades.Framework.Web.Security.Execution.Composites;
using Pleiades.Framework.Web.Security.Execution.Steps;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Pleiades.Framework.Identity
            builder.RegisterType<IdentityUserService>().As<IIdentityUserService>().InstancePerLifetimeScope();
            builder.RegisterType<AccountLevelAuthorizationStep<SystemAuthorizationContextBase>>().InstancePerLifetimeScope();
            builder.RegisterType<AccountStatusAuthorizationStep<SystemAuthorizationContextBase>>().InstancePerLifetimeScope();
            builder.RegisterType<RoleAuthorizationStep<SystemAuthorizationContextBase>>().InstancePerLifetimeScope();
            builder.RegisterType<SimpleOwnerAuthorizationStep<OwnerAuthorizationContextBase>>().InstancePerLifetimeScope();

            // Pleiades.Framework.MembershipProvider            
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerLifetimeScope();

            // Pleiades.Framework.Web.Security
            builder.RegisterType<AggregateUserService>().As<IAggregateUserService>().InstancePerLifetimeScope();
            //builder.RegisterType<PostbackSecurityResponder>().As<ISecurityCodeResponder>().InstancePerLifetimeScope();
            builder.RegisterType<PleiadesAuthorizeAttribute>();

            // Composites
            builder.RegisterType<ChangeUserPasswordComposite>().InstancePerLifetimeScope();
            builder.RegisterType<GetUserFromContextStep>().InstancePerLifetimeScope();

            // Steps
            builder.RegisterType<AuthenticateUserByRoleStep>().InstancePerLifetimeScope();
            builder.RegisterType<ChangeUserPasswordStep>().InstancePerLifetimeScope();
            builder.RegisterType<GetUserFromContextStep>().InstancePerLifetimeScope();
            builder.RegisterType<LogoutStep>().InstancePerLifetimeScope();
        }
    }
}