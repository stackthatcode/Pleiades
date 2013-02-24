using System.Collections.Generic;
using System.Linq;
using Commerce.Persist.Model.Lists;
using Newtonsoft.Json;
using DomainCategory = Commerce.Persist.Model.Lists.Category;

namespace Commerce.Persist.Model.Lists
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