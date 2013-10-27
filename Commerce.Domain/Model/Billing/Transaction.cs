using System;

namespace Commerce.Application.Model.Billing
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }

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
        }

        public Transaction(TransactionType transactionType)
        {
            this.TransactionType = transactionType;
            this.TransactionDate = DateTime.Now;
        }

        public string ToPlainEnglish()
        {
            return 
                this.TransactionType.ToPlainEnglish() + " of " + string.Format("{0:c}", this.Amount) +
                (this.Success ? " succeeded" : " failed");            
        }
    }
}
