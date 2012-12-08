using System;
using Autofac;
using Pleiades.Injection;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Default;
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
            WebSecurityAggregateBroker.RegisterSecurityContextFactory<SecurityContextFactoryBase>();
            WebSecurityAggregateBroker.RegisterSecurityResponder<SecurityResponderBase>();
        }

        public static void RegisterSecurityContextFactory<T>() where T : ISecurityContextFactory
        {
            AuthorizationRuleRegistrationAction = (builder) =>
                builder.RegisterType<T>().As<ISecurityContextFactory>().InstancePerLifetimeScope();
        }

        public static void RegisterSecurityResponder<T>() where T : ISecurityResponder
        {
            ResponderRegistrationAction = (builder) =>
                builder.RegisterType<T>().As<ISecurityResponder>().InstancePerLifetimeScope();
        }


        public static void Build(ContainerBuilder builder)
        {
            AuthorizationRuleRegistrationAction.Invoke(builder);
            ResponderRegistrationAction.Invoke(builder);
        }

        public static ISecurityContextFactory ResolveSecurityContextFactory(this IContainer injector)
        {
            return injector.Resolve<ISecurityContextFactory>();
        }

        public static ISecurityResponder ResolveSecurityResponder(this IContainer injector)
        {
            return injector.Resolve<ISecurityResponder>();
        }
    }
}