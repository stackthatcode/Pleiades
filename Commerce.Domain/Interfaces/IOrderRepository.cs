using System;
using Commerce.Persist.Model.Orders;

namespace Commerce.Persist.Interfaces
{
    public interface IOrderRepository
    {
        OrderRequestResult SubmitOrder(OrderRequest orderRequest);
    }
}
