using System;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Providers
{
    public class PfMembershipRepositoryBroker
    {
        private static Func<IMembershipProviderRepository> RepositoryFactory { get; set; }

        public static void RegisterFactory(Func<IMembershipProviderRepository> factoryMethod)
        {
            RepositoryFactory = factoryMethod;
        }

        public static IMembershipProviderRepository Create()
        {
            if (RepositoryFactory == null)
            {
                throw new Exception(
                    "RepositoryFactory factory has not been registered with PfMembershipRepositoryBroker.  Please use RegisterFactory()");
            }

            var repository = RepositoryFactory.Invoke();
            return repository;
        }
    }
}