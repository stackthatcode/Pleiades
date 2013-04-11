using System;

namespace Commerce.Persist.Model.Billing
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        // Payment Details
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }

        // Payment Processor Response
        public bool Success { get; set; }
        public string ProcessorCode { get; set; }
        public string Details { get; set; }
        public string ReferenceCode { get; set; }
        public string OriginalReferenceCode { get; set; }

        public Transaction()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}
