using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Commerce.Application.Model.Lists
{
    [JsonObject]
    public class JsonSizeGroup
    {
        public JsonSizeGroup()
        {
            this.Sizes = new List<JsonSize>();
        }

        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public bool Default { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public List<JsonSize> Sizes { get; set; }
    }
}
