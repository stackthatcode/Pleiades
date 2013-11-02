using Autofac;
using Commerce.Initializer.Builders;
using Commerce.Initializer.Builders.Products;
using Pleiades.Application.Injection;
using Pleiades.Web.Autofac;
using Pleiades.Web.Security;
using Commerce.Application;

namespace Commerce.Initializer
{
    public class CommerceInitializerModules : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Autofac
            builder.RegisterType<AutofacContainer>().As<IContainerAdapter>().InstancePerLifetimeScope();

            // External Modules
            builder.RegisterModule<WebSecurityAggregateModule>();
            builder.RegisterModule<CommerceApplicationModule>();

            builder.RegisterType<BrandBuilder>();
            builder.RegisterType<CategoryBuilder>();
            builder.RegisterType<ColorBuilder>();
            builder.RegisterType<OrderBuilder>();
            builder.RegisterType<ProductBuilder>();

            builder.RegisterType<TatamiEstiloBuilder>();
            builder.RegisterType<BullTerrierMushinBuilder>();       
            builder.RegisterType<BullTerrierSuperStarBuilder>();
            builder.RegisterType<BullTerrierZebraBuilder>();
            builder.RegisterType<BullTerrierMastersBuilder>();
            builder.RegisterType<HayabusaHeadGearBuilder>();
            builder.RegisterType<JiuJitsuDummyBuilder>();
            builder.RegisterType<ShockDoctorMouthGuardBuilderCase>();
            builder.RegisterType<ShockDoctorGelNanoBuilder>();
            builder.RegisterType<TapoutMouthguardCaseBuilder>();                        
            
            builder.RegisterType<ShippingMethodsBuilder>();
            builder.RegisterType<SizeBuilder>();
            builder.RegisterType<StateTaxBuilder>();
            builder.RegisterType<UserBuilder>();

            builder.RegisterType<AnalyticsBuilder>();
        }
    }
}