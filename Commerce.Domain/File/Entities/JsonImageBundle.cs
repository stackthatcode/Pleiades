using System;
using Newtonsoft.Json;

namespace Commerce.Application.File.Entities
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