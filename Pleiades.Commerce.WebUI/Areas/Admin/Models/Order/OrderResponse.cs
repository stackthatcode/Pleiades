using System.Collections.Generic;

namespace Commerce.WebUI.Areas.Admin.Models.Order
{
    public class OrderResponse
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }

        public OrderResponse()
        {
            this.Messages = new List<string>();
        }


        // How do we message that:
        // Payment Failed?
        // Invalid Payment Info
        // Valid Payment Info, but still a no-no
        // Payment Succeeded

        // Inventory changed?  
        // System Error?

        // On Success from Payment Process, take the External Payment ID

        // BOUNDED CONTEXT!
        // For every CreateRequestOrder, log the results in the database, sans PCI data
        // 1.) Payment Processor Response Code

    }
}
