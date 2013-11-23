using System.Collections.Generic;
using System.Linq;

namespace Commerce.Application.Orders.Entities
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
        public static List<OrderLineGroup> ToOrderLineGroups(this IEnumerable<OrderLine> orderlines)
        {
            var orderLines = orderlines as OrderLine[] ?? orderlines.ToArray();
            var uniqueSkus = orderLines.Select(x => x.OriginalSkuCode).Distinct();
            var output = new List<OrderLineGroup>();
            foreach (var skuCode in uniqueSkus)
            {
                output.Add(new OrderLineGroup
                    {
                        SkuCode = skuCode,
                        Name = orderLines.First(x => x.OriginalSkuCode == skuCode).OriginalName,
                        Quantity = orderLines.Where(x => x.OriginalSkuCode == skuCode).Sum(x => x.Quantity),
                        Price = orderLines.First(x => x.OriginalSkuCode == skuCode).OriginalUnitPrice
                    });
            }
            return output;
        }
    }
}
