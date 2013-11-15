using Newtonsoft.Json;

namespace Commerce.Application.Products.Entities
{
    [JsonObject]
    public class JsonProductImage
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string ImageBundleExternalId { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public int? ProductColorId { get; set; }

        [JsonProperty]
        public int Order { get; set; }
    }
}
