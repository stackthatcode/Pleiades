using System.Collections.Generic;
using Commerce.Application.Orders.Entities;
using System.Linq;

namespace Commerce.Application.Email.Model
{
    public class OrderLineGroup
    {
        public string SkuCode { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public static class OrderLineExtensions
    {
        public static List<OrderLineGroup> ToOrderLineGroupList(this IEnumerable<OrderLine> orderlines)
        {
            var uniqueSkus = orderlines.Select(x => x.OriginalSkuCode).Distinct();
            var output = new List<OrderLineGroup>();
            foreach (var skuCode in uniqueSkus)
            {
                output.Add(new OrderLineGroup
                    {
                        SkuCode = skuCode,
                        Name = orderlines.First(x => x.OriginalSkuCode == skuCode).OriginalSkuCode,
                        Quantity = orderlines.Sum(x => x.Quantity),
                        Price = orderlines.First(x => x.OriginalSkuCode == skuCode).OriginalUnitPrice
                    });
            }
            return output;
        }
    }
}
