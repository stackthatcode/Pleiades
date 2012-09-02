using System;
using Autofac;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web
{
    public static class WebInjectionBroker
    {
        public const string SystemAuthorizationStepKey = "SystemAuthorizationStep";

        static Action<ContainerBuilder> AuthorizationRuleRegistrationAction;
        static Action<ContainerBuilder> ResponderRegistrationAction;
        static Action<ContainerBuilder> SystemAuthorizationRegistrationAction;

        static WebInjectionBroker()
        {
            WebInjectionBroker.RegisterAuthContextBuilder<DefaultAuthorizationContextBuilder>();
            WebInjectionBroker.RegisterPostbackResponder<DefaultPostbackSecurityResponder>();
            WebInjectionBroker.RegisterSystemAuthorizationStep<DefaultAuthorizationStep>();
        }

        public static void RegisterAuthContextBuilder<T>() where T : ISystemAuthorizationContextBuilder
        {
            AuthorizationRuleRegistrationAction = (builder) =>
                builder.RegisterType<T>().As<DefaultAuthorizationContextBuilder>().InstancePerLifetimeScope();
        }

        public static void RegisterSystemAuthorizationStep<T>() where T : IStep<ISystemAuthorizationContext>
        {
            SystemAuthorizationRegistrationAction = (builder) => builder
                    .RegisterType<T>()
                    .Keyed<IStep<ISystemAuthorizationContext>>(SystemAuthorizationStepKey)
                    .InstancePerLifetimeScope();
        }

        public static void RegisterPostbackResponder<T>() where T : IPostbackSecurityResponder
        {
            ResponderRegistrationAction = (builder) =>
                builder.RegisterType<T>().As<IPostbackSecurityResponder>().InstancePerLifetimeScope();
        }


        public static void Build(ContainerBuilder builder)
        {
            AuthorizationRuleRegistrationAction.Invoke(builder);
            ResponderRegistrationAction.Invoke(builder);
            SystemAuthorizationRegistrationAction.Invoke(builder);
        }

        public static ISystemAuthorizationContextBuilder ResolveAuthContextBuilder(this IGenericContainer injector)
        {
            return injector.Resolve<ISystemAuthorizationContextBuilder>();
        }

        public static IStep<ISystemAuthorizationContext> ResolveSystemAuthorizationStep(this IGenericContainer injector)
        {
            return injector.ResolveKeyed<IStep<ISystemAuthorizationContext>>(SystemAuthorizationStepKey);
        }

        public static IPostbackSecurityResponder ResolvePostbackResponder(this IGenericContainer injector)
        {
            return injector.Resolve<IPostbackSecurityResponder>();
        }
    }
}