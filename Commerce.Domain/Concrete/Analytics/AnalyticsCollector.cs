using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Application.Database;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Analytics;
using Commerce.Application.Model.Orders;

namespace Commerce.Application.Concrete.Analytics
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

        public void Refund(DateTime date, int orderId, List<RefundItem> items)
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
                        Amount = items.Sum(x => x.Quantity * x.UnitPrice),
                        Date = date,
                        OrderId = orderId,
                        Quantity = items.Sum(x => x.Quantity)
                    }
                );
        }
    }
}
