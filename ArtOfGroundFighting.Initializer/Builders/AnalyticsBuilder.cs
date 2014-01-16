using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Analytics;
using Commerce.Application.Analytics.Entities;
using Commerce.Application.Database;
using Commerce.Application.Orders.Entities;
using Commerce.Application.Products.Entities;
using Pleiades.App.Data;
using Pleiades.App.Logging;

namespace ArtOfGroundFighting.Initializer.Builders
{
    public class AnalyticsBuilder : IBuilder
    {
        private IUnitOfWork _unitOfWork;
        private PushMarketContext _pushMarketContext;
        private IAnalyticsCollector _analyticsCollector;

        public AnalyticsBuilder(
                IUnitOfWork unitOfWork, 
                IAnalyticsCollector analyticsCollector, 
                PushMarketContext pushMarketContext)
        {
            _unitOfWork = unitOfWork;
            _analyticsCollector = analyticsCollector;
            _pushMarketContext = pushMarketContext;
        }

        public void Run()
        {
            LoggerSingleton.Get().Info("Create Analytics Test Data");
            var startDate = DateTime.Today.AddMonths(-3);
            var endDate = DateTime.Today;
            var productSkus = _pushMarketContext.ProductSkus.ToList();
            var topSellers = PickTopSellers(productSkus);

            foreach (var sku in topSellers)
            {
                PopulateSalesAndRefunds(sku, startDate, endDate);
                _unitOfWork.SaveChanges();
            }
        }

        public List<ProductSku> PickTopSellers(List<ProductSku> input)
        {
            var random = new Random();
            var output = new List<ProductSku>();
            for (int counter = 0; counter < 10; counter++)
            {
                var index = random.Next(0, input.Count);
                if (output.All(x => x.SkuCode != input[index].SkuCode))
                {
                    output.Add(input[index]);
                }
            }
            return output;
        }

        public void PopulateSalesAndRefunds(ProductSku productSku, DateTime startDate, DateTime endDate)
        {
            LoggerSingleton.Get().Info("Create Analytics Test Data for Sku: " + productSku.SkuCode);

            var random = new Random();
            for (var current = startDate; current <= endDate; current = current.AddDays(random.Next(0, 5)))
            {
                LoggerSingleton.Get().Info("Date: " + current);

                var orderId = random.Next(10000, 99999);
                var order = new Order()
                    {
                        Id = orderId,
                        DateCreated = current
                    };
                var quantity = random.Next(1, 4);
                order.OrderLines.Add(new OrderLine(productSku, quantity));

                if (random.Next(1, 6) == 5)
                {
                    var refundedQuantity = 1;

                    _analyticsCollector.Refund(
                        current,
                        orderId,
                        order.OrderLines.Sum(x => refundedQuantity * x.OriginalUnitPrice),
                        new List<RefundItem>()
                            {
                                new RefundItem
                                    {
                                        Quantity = refundedQuantity, 
                                        SkuCode = productSku.SkuCode, 
                                        UnitPrice = productSku.Product.UnitPrice
                                    }
                            } );
                }

                _analyticsCollector.Sale(order);
            }
        }
    }
}
