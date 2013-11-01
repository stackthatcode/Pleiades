using System;

namespace Commerce.Application.Model.Analytics
{
    public class PurchaseSkuEvent
    {
        public int Id { get; set; }
        public string SkuCode { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
