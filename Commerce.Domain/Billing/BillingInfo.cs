using Newtonsoft.Json;

namespace Commerce.Application.Billing
{
    [JsonObject]
    public class BillingInfo
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Address1 { get; set; }

        [JsonProperty]
        public string Address2 { get; set; }

        [JsonProperty]
        public string City { get; set; }

        [JsonProperty]
        public string State { get; set; }

        [JsonProperty]
        public string ZipCode { get; set; }

        [JsonProperty]
        public string CardNumber { get; set; }

        [JsonProperty]
        public string CVV { get; set; }

        [JsonProperty]
        public string ExpirationMonth { get; set; }

        [JsonProperty]
        public string ExpirationYear { get; set; }
    }
}
