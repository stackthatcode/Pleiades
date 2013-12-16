using Autofac;
using Commerce.Application.Azure.Config;
using Commerce.Application.Azure.File;
using Commerce.Application.File;

namespace Commerce.Application.Azure
{
    public class CommerceApplicationAzureModule : Module
    {
        // So, if the configuration is set to load Azure, these will run and wipeout the conflicting
        // ... registrations for Infrastructure stuff.  Great!
        protected override void Load(ContainerBuilder builder)
        {
            var configuration = AzureConfiguration.Settings;

            // Resource Repositories
            builder.Register(ctx => new AzureFileResourceRepository(
                        configuration.StorageConnectionString, configuration.StorageContainerName))
                .As<IFileResourceRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
