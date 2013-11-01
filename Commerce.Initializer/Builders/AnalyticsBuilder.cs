using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Commerce.Application.Database;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Analytics;
using Commerce.Application.Model.Lists;
using Commerce.Application.Model.Orders;
using Commerce.Application.Model.Products;
using Pleiades.Application.Data;
using Pleiades.Application.Logging;

namespace Commerce.Initializer.Builders
{
    public class AnalyticsBuilder : IBuilder
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<ProductSku> _inventoryRepository;
        private IAnalyticsCollector _analyticsCollector;

        public AnalyticsBuilder(
                IUnitOfWork unitOfWork, 
                IGenericRepository<ProductSku> inventoryRepository, 
                IAnalyticsCollector analyticsCollector)
        {
            _unitOfWork = unitOfWork;
            _inventoryRepository = inventoryRepository;
            _analyticsCollector = analyticsCollector;
        }

        public void Run()
        {
            using (var tx = new TransactionScope())
            {
                LoggerSingleton.Get().Info("Create the default Categories");
                var startDate = DateTime.Today.AddMonths(-3);
                var endDate = DateTime.Today;
                var productSkus = _inventoryRepository.GetAll().ToList();
                var topSellers = PickTopSellers(productSkus);

                foreach (var topSeller in topSellers)
                {
                    
                }
                _unitOfWork.SaveChanges();
                tx.Complete();
            }
        }

        public List<ProductSku> PickTopSellers(List<ProductSku> input)
        {
            var random = new Random();
            var output = new List<ProductSku>();
            for (int counter = 0; counter < 10; counter++)
            {
                var index = random.Next(0, input.Count - 1);
                if (output.All(x => x.SkuCode != input[index].SkuCode))
                {
                    output.Add(input[index]);
                }
            }
            return output;
        }

        public void PopulateSales(ProductSku productSku, DateTime startDate, DateTime endDate)
        {
            var random = new Random();
            for (var current = startDate; current <= endDate; current = current.AddDays(random.Next(0, 3)))
            {
                var order = new Order()
                    {
                        DateCreated = current
                    };
                var quantity = random.Next(1, 3);
                order.OrderLines.Add(new OrderLine(productSku, quantity));

                if (random.Next(1, 5) == 5)
                {
                    _analyticsCollector.Refund(
                        current.AddDays(
                            random.Next(3, 14)), 
                            random.Next(10000, 99999), 
                            new List<RefundItem>()
                                {
                                    new RefundItem
                                        {
                                            Quantity = 1, 
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
