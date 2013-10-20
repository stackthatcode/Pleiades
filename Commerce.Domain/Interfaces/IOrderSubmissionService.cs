using System;
using Commerce.Application.Model.Orders;

namespace Commerce.Application.Interfaces
{
    public interface IOrderSubmissionService
    {
        SubmitOrderResult Submit(OrderRequest orderRequest);
    }
}
