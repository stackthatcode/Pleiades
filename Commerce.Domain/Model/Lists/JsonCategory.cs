using System.Collections.Generic;
using System.Linq;
using Commerce.Domain.Model.Lists;
using Newtonsoft.Json;
using DomainCategory = Commerce.Domain.Model.Lists.Category;

namespace Commerce.Domain.Model.Lists
{
    [JsonObject]
    public class JsonCategory
    {
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