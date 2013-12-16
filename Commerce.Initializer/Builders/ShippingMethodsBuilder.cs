using System.Transactions;
using Commerce.Application.Database;
using Commerce.Application.Orders.Entities;
using Pleiades.App.Logging;

namespace ArtOfGroundFighting.Initializer.Builders
{
    public class ShippingMethodsBuilder : IBuilder
    {
        private PushMarketContext _context;

        public ShippingMethodsBuilder(PushMarketContext context)
        {
            _context = context;
        }

        public void Run()
        {
            using (var tx = new TransactionScope())
            {
                LoggerSingleton.Get().Info("Create the default Shipping Methods");

                _context.ShippingMethods.Add(new ShippingMethod { Description = "UPS Ground (7-10 days)", Cost = 7.95m });
                _context.ShippingMethods.Add(new ShippingMethod { Description = "UPS Ground Quicker (3-5 days) ", Cost = 14.95m });
                _context.ShippingMethods.Add(new ShippingMethod { Description = "UPS Ground Express (next day)", Cost = 21.95m });
                _context.SaveChanges();
                tx.Complete();
            }
        }
    }
}
