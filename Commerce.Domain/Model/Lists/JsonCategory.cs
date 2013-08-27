using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Model.Lists;
using Newtonsoft.Json;
using DomainCategory = Commerce.Application.Model.Lists.Category;

namespace Commerce.Application.Model.Lists
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