using Autofac;
using Commerce.Application;
using Commerce.Application.Azure;
using Pleiades.Web.Security;

namespace Commerce.IntegrationTests
{
    public class TestContainer
    {
        private static readonly ILifetimeScope RootScope;
        private static readonly ILifetimeScope AzureRootScope;

        static TestContainer()
        {
            // Non-Azure Root Scope
            RootScope = CreateRootScope(false);
            
            // Azure Root Scope
            AzureRootScope = CreateRootScope(true);
        }

        static private ILifetimeScope CreateRootScope(bool registerAzure)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceApplicationModule>();
            builder.RegisterModule<WebSecurityAggregateModule>();
            if (registerAzure)
            {
                CommerceApplicationModule.RegisterAzureComponents(builder);
            }
            return builder.Build();
        }

        public static ILifetimeScope LifetimeScope()
        {
            return RootScope.BeginLifetimeScope();
        }

        public static ILifetimeScope AzureLifetimeScope()
        {
            return AzureRootScope.BeginLifetimeScope();
        }
    }
}
