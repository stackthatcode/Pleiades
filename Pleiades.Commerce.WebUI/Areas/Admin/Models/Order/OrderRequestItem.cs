using Newtonsoft.Json;

namespace Commerce.WebUI.Areas.Admin.Models.Order
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