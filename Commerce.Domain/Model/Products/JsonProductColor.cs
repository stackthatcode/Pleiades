using Newtonsoft.Json;

namespace Commerce.Application.Model.Products
{
    [JsonObject]
    public class JsonProductColor
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string SkuCode { get; set; }

        [JsonProperty]
        public string SEO { get; set; }

        [JsonProperty]
        public string ImageBundleExternalId { get; set; }

        [JsonProperty]
        public int Order { get; set; }
    }
}
