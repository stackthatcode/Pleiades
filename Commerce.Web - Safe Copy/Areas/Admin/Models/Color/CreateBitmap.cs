using System;
using Newtonsoft.Json;

namespace Commerce.Web.Areas.Admin.Models.Color
{
    [JsonObject]
    public class CreateColorBitmap
    {
        [JsonProperty]
        public string Rgb { get; set; }

        [JsonProperty]
        public int Width{ get; set; }

        [JsonProperty]
        public int Height { get; set; }
    }
}