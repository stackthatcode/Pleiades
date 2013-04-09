using Newtonsoft.Json;

namespace Commerce.Persist.Model.Orders
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