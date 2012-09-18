using System;
using Autofac;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security
{
    public static class WebSecurityAggregateBroker
    {
        static Action<ContainerBuilder> AuthorizationRuleRegistrationAction;
        static Action<ContainerBuilder> ResponderRegistrationAction;

        static WebSecurityAggregateBroker()
        {
            WebSecurityAggregateBroker.RegisterAuthorizationContextBuilder<DefaultAuthorizationContextBuilder>();
            WebSecurityAggregateBroker.RegisterPostbackResponder<DefaultPostbackSecurityResponder>();
        }

        public static void RegisterAuthorizationContextBuilder<T>() where T : ISystemAuthorizationContextBuilder
        {
            AuthorizationRuleRegistrationAction = (builder) =>
                builder.RegisterType<T>().As<ISystemAuthorizationContextBuilder>().InstancePerLifetimeScope();
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
        }

        public static ISystemAuthorizationContextBuilder ResolveAuthorizationContextBuilder(this IServiceLocator injector)
        {
            return injector.Resolve<ISystemAuthorizationContextBuilder>();
        }

        public static IPostbackSecurityResponder ResolvePostbackResponder(this IServiceLocator injector)
        {
            return injector.Resolve<IPostbackSecurityResponder>();
        }
    }
}