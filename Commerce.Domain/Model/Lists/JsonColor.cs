using System;
using Newtonsoft.Json;
using Commerce.Application.Model.Resources;

namespace Commerce.Application.Model.Lists
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
