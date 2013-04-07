namespace Commerce.Persist.Model.Billing
{
    public class PaymentProcessorResponse
    {
        public bool Success { get; set; }
        public string ProcessorCode { get; set; }
        public string Details { get; set; }
    }
}
