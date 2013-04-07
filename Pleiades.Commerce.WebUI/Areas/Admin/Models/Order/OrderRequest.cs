using System.Collections.Generic;
using Commerce.Persist.Model.Billing;
using Commerce.Persist.Model.Orders;
using Newtonsoft.Json;

namespace Commerce.WebUI.Areas.Admin.Models.Order
{
    [JsonObject]
    public class OrderRequest
    {
        [JsonProperty]
        public ShippingInfo ShippingInfo { get; set; }

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
