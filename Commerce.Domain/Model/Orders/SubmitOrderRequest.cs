using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Model.Billing;
using Commerce.Application.Model.Orders;
using Newtonsoft.Json;

namespace Commerce.Application.Model.Orders
{
    [JsonObject]
    public class SubmitOrderRequest
    {
        [JsonProperty]
        public OrderRequestShipping ShippingInfo { get; set; }

        [JsonProperty]
        public BillingInfo BillingInfo { get; set; }

        [JsonProperty]
        public List<OrderRequestItem> Items { get; set; }

        public SubmitOrderRequest()
        {
            this.Items = new List<OrderRequestItem>();
        }

        public List<string> AllSkuCodes
        {
            get
            {
                return Items.Select(x => x.SkuCode).ToList();
            }
        }
    }
}