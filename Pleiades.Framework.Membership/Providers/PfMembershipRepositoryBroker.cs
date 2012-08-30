using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Providers
{
    public class PfMembershipRepositoryBroker
    {
        private static Func<PfMembershipProviderSettings, IMembershipProviderRepository> RepositoryFactory { get; set; }

        public static void Register(Func<PfMembershipProviderSettings, IMembershipProviderRepository> factoryMethod)
        {
            RepositoryFactory = factoryMethod;
        }

        public static IMembershipProviderRepository Create(PfMembershipProviderSettings settings)
        {
            if (RepositoryFactory == null)
            {
                throw new Exception("RepositoryFactory factory has not been registered with PfMembershipRepositoryBroker");
            }
            return RepositoryFactory.Invoke(settings);
        }
    }
}