using System.Collections.Generic;
using Commerce.Application.Products;

namespace Commerce.Application.Orders.Entities
{
    public class OrderLine
    {
        public int Id { get; set; }
        public string OriginalSkuCode { get; set; }
        public string OriginalName { get; set; }
        public decimal OriginalUnitPrice { get; set; }
        public int Quantity { get; set; }

        public ProductSku Sku { get; set; } // This guy may or may not exist...

        public OrderLine()
        {
        }

        public OrderLine(ProductSku sku, int quantity)
        {
            this.Sku = sku;
            this.OriginalSkuCode = sku.SkuCode;
            this.OriginalName = sku.Name;
            this.OriginalUnitPrice = sku.Product.UnitPrice;
            this.Quantity = quantity;
            this.Status = OrderLineStatus.Pending;
        }

        public OrderLineStatus Status
        {
            get { return (OrderLineStatus)OrderLineStatusValue; }
            set { OrderLineStatusValue = (int)value; }
        }

        public string StatusDescription
        {
            get { return Status.ToString(); }
        }

        public int OrderLineStatusValue { get; set; }

        public decimal LinePrice
        {
            get { return Quantity*OriginalUnitPrice; }
        }


        public List<OrderLine> Split()
        {
            var output = new List<OrderLine>();
            for (int counter = 1; counter < Quantity; counter++)
            {
                output.Add(
                    new OrderLine
                        {
                            Sku = this.Sku,
                            OriginalSkuCode = this.OriginalSkuCode,
                            OriginalName = this.OriginalName,
                            OriginalUnitPrice = this.OriginalUnitPrice,
                            Quantity = 1,
                            Status = this.Status,
                        });
            }
            this.Quantity = 1;
            return output;
        }
    }
}