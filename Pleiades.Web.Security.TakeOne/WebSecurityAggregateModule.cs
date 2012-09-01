using System;
using Autofac;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Composites;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web
{
    public class WebSecuritytAggregateModule : Module
    {
        public const string SystemAuthorizationStepKey = "SystemAuthorizationStep";

        protected static Action<ContainerBuilder> AuthorizationRuleRegistration;
        protected static Action<ContainerBuilder> ResponderRegistration;
        protected static Action<ContainerBuilder> SystemAuthorizationRegistration;

        static WebSecuritytAggregateModule()
        {
            RegisterAuthorizationContextBuilder<DefaultAuthorizationContextBuilder>();
            RegisterPostbackResponder<DefaultPostbackSecurityResponder>();
            RegisterSystemAuthorizer<SystemAuthorizationComposite>();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Pleiades.Web.Security.MembershipProvider            
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerLifetimeScope();

            // Pleiades.Web.Security.Aggregate

            // Services
            builder.RegisterType<IdentityUserService>().As<IIdentityUserService>().InstancePerLifetimeScope();
            builder.RegisterType<AggregateUserService>().As<IAggregateUserService>().InstancePerLifetimeScope();

            // Authorization Steps
            builder.RegisterType<AccountLevelAuthorizationStep>().InstancePerLifetimeScope();
            builder.RegisterType<AccountStatusAuthorizationStep>().InstancePerLifetimeScope();
            builder.RegisterType<RoleAuthorizationStep>().InstancePerLifetimeScope();
            builder.RegisterType<SimpleOwnerAuthorizationStep<OwnerAuthorizationContext>>().InstancePerLifetimeScope();

            // State-changing Steps
            builder.RegisterType<AuthenticateUserByRoleStep>().InstancePerLifetimeScope();
            builder.RegisterType<ChangeUserPasswordStep>().InstancePerLifetimeScope();
            builder.RegisterType<GetUserFromContextStep>().InstancePerLifetimeScope();
            builder.RegisterType<LogoutStep>().InstancePerLifetimeScope();
            
            // Composites Steps
            builder.RegisterType<ChangeUserPasswordComposite>().InstancePerLifetimeScope();
            builder.RegisterType<GetUserFromContextStep>().InstancePerLifetimeScope();

            // Register the Responder and the AuthorizationRule -- MOVE THIS TO PLEIADES.WEB?
            AuthorizationRuleRegistration(builder);
            ResponderRegistration(builder);
            SystemAuthorizationRegistration(builder);
        }


        public static void RegisterAuthorizationContextBuilder<T>() where T : ISystemAuthorizationContextBuilder
        {
            AuthorizationRuleRegistration = (builder) => 
                builder.RegisterType<T>().As<ISystemAuthorizationContextBuilder>().InstancePerLifetimeScope();
        }

        public static void RegisterPostbackResponder<T>() where T : IPostbackSecurityResponder
        {
            ResponderRegistration = (builder) => 
                builder.RegisterType<T>().As<IPostbackSecurityResponder>().InstancePerLifetimeScope();
        }

        public static void RegisterSystemAuthorizer<T>() where T : StepComposite<SystemAuthorizationContext>
        {
            SystemAuthorizationRegistration = (builder) => builder
                    .RegisterType<SystemAuthorizationComposite>()
                    .Keyed<StepComposite<SystemAuthorizationContext>>(SystemAuthorizationStepKey) 
                    .InstancePerLifetimeScope();
        }        
    }
}