using System;
using System.Transactions;
using Commerce.Persist.Database;
using Pleiades.Application.Injection;
using Pleiades.Application;
using Commerce.Persist.Concrete;
using Commerce.Persist.Model.Orders;

namespace Commerce.Initializer.Builders
{
    public class OrderBuilder
    {
        public static void Populate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                Console.WriteLine("Create the Sample Data with Orders");
                var context = ServiceLocator.Resolve<PushMarketContext>();

                // ... 

                context.SaveChanges();
                tx.Complete();
            }
        }
    }
}
