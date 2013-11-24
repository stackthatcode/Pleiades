using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Application.Injection;
using Pleiades.Web.Autofac;
using Pleiades.Web.Security;
using Commerce.Application;

namespace Commerce.Web.Plumbing
{
    public class CommerceWebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Autofac
            builder.RegisterType<AutofacContainer>().As<IContainerAdapter>().InstancePerLifetimeScope();

            // Register Commerce Controllers
            builder.RegisterControllers(typeof(CommerceWebApplication).Assembly);

            // External Modules
            builder.RegisterModule<WebSecurityAggregateModule>();
            builder.RegisterModule<CommerceApplicationModule>();

            // Aggregrate User Registration framework
            WebSecurityAggregateBroker.RegisterSecurityContextFactory<SecurityContextFactory>();
            WebSecurityAggregateBroker.RegisterSecurityResponder<SecurityResponder>();
            WebSecurityAggregateBroker.Build(builder);
        }
    }
}