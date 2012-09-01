using System;
using Autofac;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web
{
    public class WebModule : Module
    {
        public const string SystemAuthorizationStepKey = "SystemAuthorizationStep";

        protected static Action<ContainerBuilder> AuthorizationRuleRegistration;
        protected static Action<ContainerBuilder> ResponderRegistration;
        protected static Action<ContainerBuilder> SystemAuthorizationRegistration;
        
        static WebModule()
        {
            RegisterAuthContextBuilder<DefaultAuthorizationContextBuilder>();
            RegisterPostbackResponder<DefaultPostbackSecurityResponder>();
            RegisterSystemAuthorizer<DefaultAuthorizationStep>();
        }

        protected override void Load(ContainerBuilder builder)
        {           
            // Register the Responder and the AuthorizationRule
            AuthorizationRuleRegistration(builder);
            ResponderRegistration(builder);
            SystemAuthorizationRegistration(builder);
        }

        public static void RegisterAuthContextBuilder<T>() where T : DefaultAuthorizationContextBuilder
        {
            AuthorizationRuleRegistration = (builder) =>
                builder.RegisterType<T>().As<DefaultAuthorizationContextBuilder>().InstancePerLifetimeScope();
        }

        public static void RegisterPostbackResponder<T>() where T : IPostbackSecurityResponder
        {
            ResponderRegistration = (builder) => 
                builder.RegisterType<T>().As<IPostbackSecurityResponder>().InstancePerLifetimeScope();
        }

        public static void RegisterSystemAuthorizer<T>() where T : Step<ISystemAuthorizationContext>
        {
            SystemAuthorizationRegistration = (builder) => builder
                    .RegisterType<T>()
                    .Keyed<Step<ISystemAuthorizationContext>>(SystemAuthorizationStepKey) 
                    .InstancePerLifetimeScope();
        }        
    }
}