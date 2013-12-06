using Autofac;
using Autofac.Integration.Mvc;
using Commerce.Web.Plumbing;
using Pleiades.App.Injection;
using Pleiades.Web.Security;
using Commerce.Application;

namespace Commerce.Web.Autofac
{
    public class CompositionRoot : Module
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