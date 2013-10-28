using System;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Orders;

namespace Commerce.Application.Concrete.Infrastructure
{
    public class AnalyticService : IAnalyticsCollector
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