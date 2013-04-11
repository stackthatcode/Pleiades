using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Persist.Model.Billing;

namespace Commerce.Persist.Model.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }

        // Customer Id
        public Guid CustomerId { get; set; }
        
        // Shipping Info
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }

        public ShippingMethod ShippingMethod { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public StateTax StateTax { get; set; }

        // Payment Info
        public List<Transaction> PaymentTransactions { get; set; }

        // Audit Info
        public List<string> Notes { get; set; }

        // ctor
        public Order()
        {
            this.OrderLines = new List<OrderLine>();
            this.PaymentTransactions = new List<Transaction>();
        }

        // Computed properties...
        public decimal SubTotal
        {
            get
            {
                return OrderLines.Sum(x => x.LinePrice);
            }
        }

        public decimal Tax
        {
            get
            {
                return SubTotal * this.StateTax.TaxRate;
            }
        }

        public decimal GrandTotal
        {
            get
            {
                return SubTotal + Tax + ShippingMethod.Cost;
            }
        }

        public decimal OriginalGrandTotal { get; set; }

        public decimal RefundedAmount
        {
            get
            {
                return this.PaymentTransactions
                    .Where(x => x.TransactionType == TransactionType.Refund && x.Success == true)
                    .Sum(x => x.Amount);
            }
        }

        public List<string> AllSkuCodes
        {
            get
            {
                return OrderLines.Select(x => x.OriginalSkuCode).ToList();
            }
        }
    }
}