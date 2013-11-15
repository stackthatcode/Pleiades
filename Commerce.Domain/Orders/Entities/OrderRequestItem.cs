using Newtonsoft.Json;

namespace Commerce.Application.Orders.Entities
{
    [JsonObject]
    public class OrderRequestItem
    {
        [JsonProperty]
        public string SkuCode { get; set; }

        [JsonProperty]
        public int Quantity { get; set; }
    }
}