using System;
using Autofac;
using Pleiades.Framework.Execution;
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
    public class FrameworkWebModule : Module
    {
        public const string SystemAuthorizationStepKey = "SystemAuthorizationStep";

        protected static Action<ContainerBuilder> AuthorizationRuleRegistration;
        protected static Action<ContainerBuilder> ResponderRegistration;
        protected static Action<ContainerBuilder> SystemAuthorizationRegistration;
        
        static FrameworkWebModule()
        {
            RegisterAuthContextBuilder<DefaultAuthContextBuilder>();
            RegisterPostbackResponder<DefaultResponder>();
            RegisterSystemAuthorizer<SystemAuthorizationComposite>();
        }

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
            builder.RegisterType<PleiadesAuthorizeAttribute>();
            
            // Register the Responder and the AuthorizationRule
            AuthorizationRuleRegistration(builder);
            ResponderRegistration(builder);
            SystemAuthorizationRegistration(builder);

            // Composites
            builder.RegisterType<ChangeUserPasswordComposite>().InstancePerLifetimeScope();
            builder.RegisterType<GetUserFromContextStep>().InstancePerLifetimeScope();

            // Steps
            builder.RegisterType<AuthenticateUserByRoleStep>().InstancePerLifetimeScope();
            builder.RegisterType<ChangeUserPasswordStep>().InstancePerLifetimeScope();
            builder.RegisterType<GetUserFromContextStep>().InstancePerLifetimeScope();
            builder.RegisterType<LogoutStep>().InstancePerLifetimeScope();
        }

        public static void RegisterAuthContextBuilder<T>() where T : ISystemAuthContextBuilder
        {
            AuthorizationRuleRegistration = (builder) => 
                builder.RegisterType<T>().As<ISystemAuthContextBuilder>().InstancePerLifetimeScope();
        }

        public static void RegisterPostbackResponder<T>() where T : IPostbackSecurityResponder
        {
            ResponderRegistration = (builder) => 
                builder.RegisterType<T>().As<IPostbackSecurityResponder>().InstancePerLifetimeScope();
        }

        public static void RegisterSystemAuthorizer<T>() where T : StepComposite<SystemAuthorizationContextBase>
        {
            SystemAuthorizationRegistration = (builder) => builder
                    .RegisterType<SystemAuthorizationComposite>()
                    .Keyed<StepComposite<SystemAuthorizationContextBase>>(SystemAuthorizationStepKey) 
                    .InstancePerLifetimeScope();
        }        
    }
}