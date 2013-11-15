using System.Collections.Generic;
using Newtonsoft.Json;

namespace Commerce.Application.Lists.Entities
{
    [JsonObject]
    public class JsonCategory
    {
        public JsonCategory()
        {
            this.Categories = new List<JsonCategory>();
        }

        [JsonProperty]
        public int? Id { get; set; }

        [JsonProperty]
        public int? ParentId { get; set; }

        [JsonProperty]
        public List<JsonCategory> Categories { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string SEO { get; set; }

        [JsonProperty]
        public int? NumberOfCategories { get; set; }
    }
}