﻿using System;
using Newtonsoft.Json;

namespace Commerce.Domain.Model.Lists
{
    [JsonObject]
    public class JsonSize
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string SEO { get; set; }

        [JsonProperty]
        public string SkuCode { get; set; }
    }
}
