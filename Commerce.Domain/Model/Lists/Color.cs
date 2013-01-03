using System;
using Newtonsoft.Json;

namespace Commerce.Domain.Model.Lists
{
    [JsonObject]
    public class Color
    {
        public Color()
        {
            RGB = "#FFFFFF";
        }

        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string SkuCode { get; set; }

        [JsonProperty]
        public string RGB { get; set; }

        [JsonProperty]
        public string ImageResourceId { get; set; }

        public bool Deleted { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }
    }
}