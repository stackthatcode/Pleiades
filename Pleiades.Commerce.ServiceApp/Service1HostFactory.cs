using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;

namespace Pleiades.Commerce.ServiceApp
{
    public class Service1HostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type t, Uri[] baseAddresses)
        {
            return new ServiceHost(t, baseAddresses);
        }

        public override ServiceHostBase CreateServiceHost(string service, Uri[] baseAddresses)
        {
            // The service parameter is ignored here because we know our service.
            var serviceHost = new ServiceHost(typeof(Service1Enhanced), baseAddresses);
            return serviceHost;
        }
    }
}
