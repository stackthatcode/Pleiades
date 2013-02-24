using System;
using Newtonsoft.Json;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Model.Resources
{
    [JsonObject]
    public class JsonImageBundle
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public Guid ExternalId { get; set; }


    }
}