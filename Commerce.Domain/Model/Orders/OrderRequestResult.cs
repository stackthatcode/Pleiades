using System.Collections.Generic;

namespace Commerce.Persist.Model.Orders
{
    public class OrderRequestResult
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }

        public OrderRequestResult()
        {
            this.Messages = new List<string>();
        }

        public OrderRequestResult(bool success, string message)
        {
            this.Messages = new List<string>() { message };
            this.Success = success;
        }

        // System Error?
        // On Success from Payment Process, take the External Payment ID

        // BOUNDED CONTEXT!
        // For every CreateRequestOrder, log the results in the database, sans PCI data
        // 1.) Payment Processor Response Code

    }
}
