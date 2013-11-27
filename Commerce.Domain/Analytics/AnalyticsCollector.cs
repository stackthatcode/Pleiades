using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Analytics.Entities;
using Commerce.Application.Database;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Analytics
{
    public class AnalyticsCollector : IAnalyticsCollector
    {
        private readonly PushMarketContext _context;

        public AnalyticsCollector(PushMarketContext context)
        {
            _context = context;
        }
        
        public void Sale(Order order)
        {
            _context.PurchaseOrderEvents.Add(
                new PurchaseOrderEvent()
                {
                    OrderId = order.Id,
                    SaleAmount = order.Total.GrandTotal,
                    Date = order.DateCreated,
                    Quantity = order.OrderLines.Sum(x => x.Quantity),
                });

            foreach (var skuQuantity in order.QuantityBySkuCode)
            {
                var productSku = order.AllSkus.FirstOrDefault(x => x.SkuCode == skuQuantity.Key);

                _context.PurchaseSkuEvents.Add(
                    new PurchaseSkuEvent
                        {
                            SkuCode = skuQuantity.Key,
                            Date = order.DateCreated,
                            Quantity = skuQuantity.Value,
                            Amount = skuQuantity.Value * productSku.Product.UnitPrice
                        }
                    );
            }
        }

        public void Refund(DateTime date, int orderId, decimal refundAmount, List<RefundItem> items)
        {
            foreach (var item in items)
            {
                _context.RefundSkuEvents.Add(
                    new RefundSkuEvent
                        {
                            SkuCode = item.SkuCode,
                            Amount = item.Quantity*item.UnitPrice,
                            Date = date,
                            Quantity = item.Quantity
                        }
                    );
            }

            _context.RefundEvents.Add(
                new RefundEvent
                    {
                        // NOTICE: the Refund Amount does NOT include taxes - that's at the Transaction Level
                        Amount = refundAmount,
                        Date = date,
                        OrderId = orderId,
                        Quantity = items.Sum(x => x.Quantity)
                    }
                );
        }
    }
}
