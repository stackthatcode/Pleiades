using System;

namespace Commerce.Application.Analytics.Entities
{
    public class PurchaseOrderEvent
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public decimal SaleAmount { get; set; }
    }
}
