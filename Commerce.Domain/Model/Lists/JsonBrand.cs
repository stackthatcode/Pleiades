using System;
using Newtonsoft.Json;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Model.Lists
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
    }
}
