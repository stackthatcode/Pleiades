using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Orders
{
    public interface IOrderService
    {
        SubmitOrderResult Submit(OrderRequest orderRequest);
        Order Retreive(string externalId);
    }
}
