using System;
using Commerce.Persist.Model.Orders;

namespace Commerce.Persist.Interfaces
{
    public interface IOrderService
    {
        OrderRequestResult SubmitOrderRequest(OrderRequest orderRequest);
    }
}
