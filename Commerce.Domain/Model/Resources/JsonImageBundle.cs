using System;
using Newtonsoft.Json;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Model.Resources
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