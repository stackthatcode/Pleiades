using System;
using Autofac;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Concrete;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Framework.MembershipProvider.Concrete;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.Web.Interface;
using Pleiades.Framework.Web.Security;
using Pleiades.Commerce.Web.Security.Concrete;
using Pleiades.Commerce.Web.Security.Execution.Abstract;
using Pleiades.Commerce.Web.Security.Execution.Composites;
using Pleiades.Commerce.Web.Security.Execution.Steps;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web
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

            // Concrete
            builder.RegisterType<SecurityCodeResponder>().As<ISecurityCodeFilterResponder>().InstancePerLifetimeScope();

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