﻿using System;
using Newtonsoft.Json;

namespace Commerce.Domain.Model.Lists
{
    [JsonObject]
    public class Brand
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Description { get; set; }
    }
}
