using System.Collections.Generic;
using System.Linq;

namespace Commerce.Application.Orders.Entities
{
    public class SubmitOrderResult
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
        public Order Order { get; set; }

        public SubmitOrderResult(Order order)
        {
            this.Messages = order.Notes.Select(x => x.Content).ToList();
            this.Order = order;
            this.Success = true;
        }

        public SubmitOrderResult(bool success, string message)
        {
            this.Messages = new List<string>() { message };
            this.Success = success;
        }
    }
}
