using System;
using System.Transactions;
using Commerce.Application.Database;
using Pleiades.Application.Injection;
using Pleiades.Application.Logging;

namespace Commerce.Initializer.Builders
{
    public class OrderBuilder
    {
        public static void Populate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                LoggerSingleton.Get().Info("Create the Sample Data with Orders");
                var context = ServiceLocator.Resolve<PushMarketContext>();

                // ... 

                context.SaveChanges();
                tx.Complete();
            }
        }
    }
}
