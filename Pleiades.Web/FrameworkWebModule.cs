using System;
using Autofac;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Execution;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Execution.Abstract;
using Pleiades.Web.Security.Execution.Composites;
using Pleiades.Web.Security.Execution.Steps;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web
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
            // Pleiades.Identity
            builder.RegisterType<IdentityUserService>().As<IIdentityUserService>().InstancePerLifetimeScope();
            builder.RegisterType<AccountLevelAuthorizationStep<SystemAuthorizationContextBase>>().InstancePerLifetimeScope();
            builder.RegisterType<AccountStatusAuthorizationStep<SystemAuthorizationContextBase>>().InstancePerLifetimeScope();
            builder.RegisterType<RoleAuthorizationStep<SystemAuthorizationContextBase>>().InstancePerLifetimeScope();
            builder.RegisterType<SimpleOwnerAuthorizationStep<OwnerAuthorizationContextBase>>().InstancePerLifetimeScope();

            // Pleiades.MembershipProvider            
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerLifetimeScope();

            // Pleiades.Web.Security
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