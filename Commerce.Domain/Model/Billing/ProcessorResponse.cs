namespace Commerce.Persist.Model.Billing
{
    public class ProcessorResponse
    {
        public bool Success { get; set; }
        public string ProcessorCode { get; set; }
        public string Details { get; set; }
        public string ExternalReferenceCode { get; set; }
    }
}
