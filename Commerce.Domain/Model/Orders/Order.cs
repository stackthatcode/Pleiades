using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Model.Billing;

namespace Commerce.Application.Model.Orders
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
        public DateTime DateCreated { get; set; }

        // Payment Info
        public List<Transaction> Transactions { get; set; }

        // Audit Info
        public List<OrderNote> Notes { get; set; }

        // ctor
        public Order()
        {
            this.CustomerId = Guid.NewGuid();
            this.OrderLines = new List<OrderLine>();
            this.Transactions = new List<Transaction>();
            this.Notes = new List<OrderNote>();
        }

        public void AddNote(string content)
        {
            this.Notes.Add(new OrderNote(content));
        }

        public void AddTransaction(Transaction transaction)
        {
            this.Transactions.Add(transaction);
            this.AddNote(transaction.ToPlainEnglish());
        }

        // Computed properties...
        public decimal SubTotal
        {
            get
            {
                return OrderLines.Sum(x => x.LinePrice);
            }
            set
            {
                // Do nothing!                
            }
        }

        public decimal Tax
        {
            get
            {
                return SubTotal * this.StateTax.TaxRate / 100.00m;
            }
            set
            {
                // Do nothing!
            }
        }

        public decimal ShippingCost
        {
            get
            {
                return (this.SubTotal != 0 && this.ShippingMethod != null) ? this.ShippingMethod.Cost : 0;
            }
            set
            {
                // Do nothing
            }
        }

        public decimal GrandTotal
        {
            get
            {
                return SubTotal + Tax + ShippingCost;
            }
            set
            {
                // Do nothing!
            }
        }

        public decimal OriginalGrandTotal { get; set; }

        public decimal RefundedAmount
        {
            get
            {
                return this.Transactions
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

        public bool Complete
        {
            get
            {
                return this.OrderLines.Any(x => 
                    x.Status != OrderLineStatus.Shipped && x.Status != OrderLineStatus.Received);
            }
            set 
            {
                // Do nothing
            }
        }
    }
}