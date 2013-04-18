using System;
using Commerce.Persist.Model.Orders;

namespace Commerce.Persist.Interfaces
{
    public interface IOrderSubmissionService
    {
        SubmitOrderResult Submit(SubmitOrderRequest orderRequest);
    }
}
