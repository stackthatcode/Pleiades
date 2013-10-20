using Newtonsoft.Json;

namespace Commerce.Application.Model.Orders
{
    [JsonObject]
    public class ShippingInfo
    {
        [JsonProperty]
        public string EmailAddress { get; set; }

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
        public string Phone { get; set; }

        // NOTE: this is legacy, but used by the old Knockout Submit Form
        [JsonProperty]
        public int ShippingOptionId { get; set; }
    }
}
