using Commerce.Application.Model.Products;

namespace Commerce.Application.Model.Orders
{
    public class OrderLine
    {
        public int Id { get; set; }
        public string OriginalSkuCode { get; set; }
        public string OriginalName { get; set; }
        public decimal OriginalUnitPrice { get; set; }
        public int Quantity { get; set; }
        public OrderLineStatus Status { get; set; }
        public ProductSku Sku { get; set; } // This guy may or may not exist...
        
        public OrderLine(ProductSku sku, int quantity)
        {
            this.Sku = sku;
            this.OriginalSkuCode = sku.SkuCode;
            this.OriginalName = sku.Name;
            this.OriginalUnitPrice = sku.Product.UnitPrice;
            this.Quantity = quantity;
            this.Status = OrderLineStatus.Pending;
        }

        public decimal LinePrice
        {
            get
            {
                return Quantity * OriginalUnitPrice;
            }
        }
    }
}