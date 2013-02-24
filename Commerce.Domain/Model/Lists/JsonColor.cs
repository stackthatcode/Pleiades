using System;
using Newtonsoft.Json;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Model.Lists
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
