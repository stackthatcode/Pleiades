using System.Collections.Generic;
using Commerce.Persist.Model.Billing;
using Commerce.Persist.Model.Orders;
using Newtonsoft.Json;

namespace Commerce.Persist.Model.Orders
{
    [JsonObject]
    public class OrderRequest
    {
        [JsonProperty]
        public OrderRequestShipping ShippingInfo { get; set; }

        [JsonProperty]
        public BillingInfo BillingInfo { get; set; }

        [JsonProperty]
        public List<OrderRequestItem> Items { get; set; }

        public OrderRequest()
        {
            this.Items = new List<OrderRequestItem>();
        }
    }
}
