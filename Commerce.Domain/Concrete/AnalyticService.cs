using System;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Orders;

namespace Commerce.Persist.Concrete
{
    public class AnalyticService : IAnalyticsService
    {
        public static int TestVariable = 3;

        public void AddSale(Order order1, Order order2)
        {
            throw new NotImplementedException();
        }

        public int TestProperty
        {
            get { throw new NotImplementedException(); }
        }

        public void AddSale(Order order, int cost)
        {
            // TODO: add functionality
            // this.Context.SaveChanges            
        }
    }
}