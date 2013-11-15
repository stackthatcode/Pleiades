using Newtonsoft.Json;

namespace Commerce.Application.Lists.Entities
{
    [JsonObject]
    public class JsonSize
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public int ParentGroupId { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public string SEO { get; set; }

        [JsonProperty]
        public string SkuCode { get; set; }
    }
}
