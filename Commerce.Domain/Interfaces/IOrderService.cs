using Commerce.Application.Model.Orders;

namespace Commerce.Application.Interfaces
{
    public interface IOrderService
    {
        SubmitOrderResult Submit(OrderRequest orderRequest);
        Order Retreive(string externalId);
    }
}
