using System;

namespace Commerce.Application.Model.Billing
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }

        // Payment Details
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }

        // Payment Processor Response
        public bool Success { get; set; }
        public string OriginalReferenceCode { get; set; }
        public string ReferenceCode { get; set; }

        public string ProcessorResponse { get; set; }
        public string Details { get; set; }

        public Transaction()
        {            
        }

        public Transaction(int transactionType)
        {
            this.TransactionType = transactionType;
            this.TransactionDate = DateTime.Now;
        }

        public string TransactionTypeDescription
        {
            get { return TransactionType.ToPlainEnglish(); }
        }

        public string ToPlainEnglish()
        {
            return 
                this.TransactionType.ToPlainEnglish() + " of " + string.Format("{0:c}", this.Amount) +
                (this.Success ? " succeeded" : " failed");            
        }
    }
}
