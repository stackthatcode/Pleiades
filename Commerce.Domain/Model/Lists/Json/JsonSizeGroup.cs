using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Commerce.Domain.Model.Lists.Json
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
        public string Name { get; set; }

        [JsonProperty]
        public List<JsonSize> Sizes { get; set; }
    }
}
