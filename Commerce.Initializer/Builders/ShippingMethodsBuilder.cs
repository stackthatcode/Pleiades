using System;
using System.Transactions;
using Pleiades.Injection;
using Pleiades.Data;
using Commerce.Persist.Concrete;
using Commerce.Persist.Model.Orders;

namespace Commerce.Initializer.Builders
{
    public class ShippingMethodsBuilder
    {
        public static void EmptyAndRepopulate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                Console.WriteLine("Create the default Shipping Methods");

                var context = ServiceLocator.Resolve<PleiadesContext>();

                context.ShippingMethods.Add(new ShippingMethod { Description = "UPS Ground (7-10 days)", Cost = 7.95m });
                context.ShippingMethods.Add(new ShippingMethod { Description = "UPS Ground Quicker (3-5 days) ", Cost = 14.95m });
                context.ShippingMethods.Add(new ShippingMethod { Description = "UPS Ground Express (next day)", Cost = 21.95m });

                context.SaveChanges();
                tx.Complete();
            }
        }
    }
}
