using Autofac;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security
{
    public class WebSecurityAggregateModule : Module
    {       
        protected override void Load(ContainerBuilder builder)
        {
            // Pleiades.Web.Security.MembershipProvider            
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<PfMembershipService>().As<IPfMembershipService>().InstancePerLifetimeScope();
            builder.RegisterType<PfPasswordService>().As<IPfPasswordService>().InstancePerLifetimeScope();
            builder.RegisterType<PfMembershipSettings>().As<IPfMembershipSettings>().InstancePerLifetimeScope();

            // Concrete Services
            builder.RegisterType<AggregateUserService>().As<IAggregateUserService>().InstancePerLifetimeScope();
            
            // Aspect
            builder.RegisterType<SecurityAttribute>().InstancePerLifetimeScope();
        }   
    }
}