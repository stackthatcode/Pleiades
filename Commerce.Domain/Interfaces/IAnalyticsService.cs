using Commerce.Persist.Model.Orders;

namespace Commerce.Persist.Interfaces
{
    public interface IAnalyticsService
    {
        void AddSale(Order order, int cost);
        void AddSale(Order order1, Order order2);
        int TestProperty { get; }
    }
}