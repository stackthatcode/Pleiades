using System;
using Autofac;
using Pleiades.Web.Security.Default;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security
{
    public static class WebSecurityAggregateBroker
    {
        static Action<ContainerBuilder> AuthorizationRuleRegistrationAction;
        static Action<ContainerBuilder> ResponderRegistrationAction;

        static WebSecurityAggregateBroker()
        {
            WebSecurityAggregateBroker.RegisterSecurityContextFactory<DefaultSecurityContextBuilder>();
            WebSecurityAggregateBroker.RegisterSecurityResponder<DefaultSecurityHttpResponder>();
        }

        public static void RegisterSecurityContextFactory<T>() where T : ISecurityContextBuilder
        {
            AuthorizationRuleRegistrationAction = (builder) =>
                builder.RegisterType<T>().As<ISecurityContextBuilder>().InstancePerLifetimeScope();
        }

        public static void RegisterSecurityResponder<T>() where T : ISecurityHttpResponder
        {
            ResponderRegistrationAction = (builder) =>
                builder.RegisterType<T>().As<ISecurityHttpResponder>().InstancePerLifetimeScope();
        }


        public static void Build(ContainerBuilder builder)
        {
            AuthorizationRuleRegistrationAction.Invoke(builder);
            ResponderRegistrationAction.Invoke(builder);
        }

        public static ISecurityContextBuilder ResolveSecurityContextFactory(this IContainer injector)
        {
            return injector.Resolve<ISecurityContextBuilder>();
        }

        public static ISecurityHttpResponder ResolveSecurityResponder(this IContainer injector)
        {
            return injector.Resolve<ISecurityHttpResponder>();
        }
    }
}