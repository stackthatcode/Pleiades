using Newtonsoft.Json;

namespace Commerce.Application.Lists.Entities
{
    [JsonObject]
    public class JsonBrand
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public string SkuCode { get; set; }

        [JsonProperty]
        public string SEO { get; set; }

        [JsonProperty]
        public string ImageBundleExternalId { get; set; }

        [JsonProperty]
        public int ProductCount { get; set; }
    }
}
