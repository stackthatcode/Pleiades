using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Commerce.Application.Orders.Entities
{
    [JsonObject]
    public class OrderRequest
    {
        [JsonProperty]
        public ShippingInfo ShippingInfo { get; set; }

        [JsonProperty]
        public string Token { get; set; }

        [JsonProperty]
        public List<OrderRequestItem> Items { get; set; }

        public OrderRequest()
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
