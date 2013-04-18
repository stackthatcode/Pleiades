using System.Collections.Generic;
using System.Linq;

namespace Commerce.Persist.Model.Orders
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


        // System Error? => no External Payment ID
        // On Success from Payment Process, take the External Payment ID

        // BOUNDED CONTEXT!
        // For every CreateRequestOrder, log the results in the database, sans PCI data
        // 1.) Payment Processor Response Code
    }
}
