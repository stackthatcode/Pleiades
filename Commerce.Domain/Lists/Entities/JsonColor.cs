using Newtonsoft.Json;

namespace Commerce.Application.Lists.Entities
{
    [JsonObject]
    public class JsonColor
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
    }
}
