using System;
using System.Collections.Generic;
using Commerce.Application.Analytics;
using Commerce.Application.Billing;
using Commerce.Application.Email;
using Commerce.Application.Orders;
using Commerce.Application.Orders.Entities;
using Commerce.Application.Payment;
using Commerce.Application.Products;
using Commerce.Application.Products.Entities;
using NUnit.Framework;
using Rhino.Mocks;

namespace Commerce.UnitTests.Application
{
    [TestFixture]
    public class OrderSubmissionServiceTests
    {
        private OrderRequest _orderRequest;

        [SetUp]
        public void Setup()
        {
            var item1 = new OrderRequestItem()
            {
                Quantity = 5,
                SkuCode = "PROD1-RED-MED",
            };

            var item2 = new OrderRequestItem()
            {
                Quantity = 3,
                SkuCode = "PROD2-BLUE-MED",
            };

            var item3 = new OrderRequestItem()
            {
                Quantity = 3,
                SkuCode = "PROD3",
            };

            _orderRequest = new OrderRequest()
            {
                Token = "TESTTOKEN",
                ShippingInfo = new ShippingInfo(),
                Items = new List<OrderRequestItem>
                {
                    item1,
                    item2, 
                    item3,
                }
            };
        }

        [Test]
        public void Verify_That_Missing_Info_Causes_Failure()
        {
            // Arrange
            var request = _orderRequest;
            request.Token = null;
            var service = new OrderService(null, () => null, null, null, null);

            // Act
            var result = service.Submit(request);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Messages, Contains.Item(OrderService.ErrorMissingData));
            result.Messages.ForEach(Console.WriteLine);
        }
        
        [Test]
        public void Verify_That_Failed_Payment_Causes_Failure()
        {
            // Arrange
            var request = _orderRequest;
            var paymentProcessor = MockRepository.GenerateMock<IPaymentProcessor>();
            paymentProcessor
                .Expect(x => x.Charge(null, 206.975M))
                .Return(new Transaction(TransactionType.AuthorizeAndCollect) { Success = false })
                .IgnoreArguments();

            var service = new OrderService(null, () => paymentProcessor, null, null, null);
            service.InventoryBySkuCodes = this.SkuFunctionGenerator_PlainVanilla();
            service.StateTaxByAbbr = this.StateTaxFunctionGenerator();
            service.ShippingMethodById = this.ShippingMethodFunctionGenerator();

            // Act
            var result = service.Submit(request);

            // Assert
            paymentProcessor.VerifyAllExpectations();
            Assert.That(result.Success, Is.False);
            Assert.That(result.Messages, Contains.Item(OrderService.ErrorFailedPayment));
            result.Messages.ForEach(x => Console.WriteLine(x));
        }

        [Test]
        public void Verify_That_Exception_Throw_Causes_Failure()
        {
            // Arrange
            var request = _orderRequest;
            var service = new OrderService(null, () => null, null, null, null);

            // Act
            var result = service.Submit(request);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Messages, Contains.Item(OrderService.ErrorFault));
            result.Messages.ForEach(x => Console.WriteLine(x));
        }

        [Test]
        public void Verify_That_Successful_Payment_With_Plain_Vanilla_Inventory_Succeeds()
        {
            // Arrange
            var request = _orderRequest;
            var paymentProcessor = MockRepository.GenerateMock<IPaymentProcessor>();
            paymentProcessor
                .Expect(x => x.Charge(_orderRequest.Token, 206.975m))
                .Return(new Transaction(TransactionType.AuthorizeAndCollect) {Success = true, Amount = 206.975m});

            var emailGenerator = MockRepository.GenerateMock<IEmailGenerator>();
            var message = new EmailMessage();
            emailGenerator.Expect(x => x.OrderReceived(null)).IgnoreArguments().Return(message);
            var emailService = MockRepository.GenerateMock<IEmailService>();
            emailService.Expect(x => x.Send(message));

            var analyticsService = MockRepository.GenerateMock<IAnalyticsCollector>();
            analyticsService.Expect(x => x.Sale(null)).IgnoreArguments();   // Oh my!

            var service = new OrderService(null, () => paymentProcessor, analyticsService, emailService, emailGenerator);
            service.InventoryBySkuCodes = this.SkuFunctionGenerator_PlainVanilla();
            service.StateTaxByAbbr = this.StateTaxFunctionGenerator();
            service.ShippingMethodById = this.ShippingMethodFunctionGenerator();
            service.CreateOrder = (order) => { };
            service.SaveChanges = () => { };
            
            // Act
            var result = service.Submit(request);

            // Assert
            paymentProcessor.VerifyAllExpectations();
            Assert.That(result.Success, Is.True);
            result.Messages.ForEach(x => Console.WriteLine(x));
        }

        [Test]
        public void Verify_That_Failed_Payment_With_Changing_Inventory_Succeeds_And_Issues_Refund()
        {
            // Arrange
            var request = _orderRequest;

            var paymentProcessor = MockRepository.GenerateMock<IPaymentProcessor>();
            paymentProcessor
                .Expect(x => x.Charge(null, 206.975m))
                .Return(new Transaction(TransactionType.AuthorizeAndCollect) { Success = true, Amount = 206.975m })
                .IgnoreArguments();
            paymentProcessor
                .Expect(x => x.Refund(null, 66.66m))
                .Return(new Transaction(TransactionType.Refund) { Success = true, Amount = 66.66m })
                .IgnoreArguments();

            var emailGenerator = MockRepository.GenerateMock<IEmailGenerator>();
            var message = new EmailMessage();
            emailGenerator.Expect(x => x.OrderReceived(null)).IgnoreArguments().Return(message);
            var emailService = MockRepository.GenerateMock<IEmailService>();

            var analyticsService = MockRepository.GenerateMock<IAnalyticsCollector>();
            analyticsService.Expect(x => x.Sale(null)).IgnoreArguments();

            var service = new OrderService(null, () => paymentProcessor, analyticsService, emailService, emailGenerator);
            service.InventoryBySkuCodes = this.SkuFunctionGenerator_ChangingInventory();
            service.StateTaxByAbbr = this.StateTaxFunctionGenerator();
            service.ShippingMethodById = this.ShippingMethodFunctionGenerator();
            service.CreateOrder = (order) => { };
            service.SaveChanges = () => { };

            // Act
            var result = service.Submit(request);

            // Assert
            paymentProcessor.VerifyAllExpectations();
            Assert.That(result.Success, Is.True);
            result.Messages.ForEach(x => Console.WriteLine(x));
        }

        [Test]
        public void Generate_New_External_Order_Numbers_For_Giggles()
        {
            Console.WriteLine(OrderNumberGenerator.Next());
        }


        // Define Injectibles
        private Func<IEnumerable<string>, bool, List<ProductSku>> SkuFunctionGenerator_PlainVanilla()
        {
            var product1 = new Product() 
            {
                Description = "Sample Product",
                UnitPrice = 25.00m,
            };

            var product2 = new Product()
            {
                Description = "Sample Product",
                UnitPrice = 10.00m,
            };

            var product3 = new Product()
            {
                Description = "Sample Product",
                UnitPrice = 10.00m,
            };

            return (skus, refresh) =>
            {
                return new List<ProductSku>
                {
                    new ProductSku { Product = product1, SkuCode = "PROD1-RED-MED", InStock = 5, Reserved = 0 },
                    new ProductSku { Product = product2, SkuCode = "PROD2-BLUE-MED", InStock = 5, Reserved = 0 },
                    new ProductSku { Product = product3, SkuCode = "PROD3", InStock = 5, Reserved = 0 },
                };
            };
        }

        private Func<IEnumerable<string>, bool, List<ProductSku>> SkuFunctionGenerator_ChangingInventory()
        {
            var product1 = new Product()
            {
                Description = "Sample Product",
                UnitPrice = 25.00m,
            };

            var product2 = new Product()
            {
                Description = "Sample Product",
                UnitPrice = 10.00m,
            };

            var product3 = new Product()
            {
                Description = "Sample Product",
                UnitPrice = 10.00m,
            };

            var invoke_count_closure = 0;
            return (skus, refresh) =>
            {
                invoke_count_closure++;
                if (invoke_count_closure == 1)
                {
                    return new List<ProductSku>
                    {
                        new ProductSku { Product = product1, SkuCode = "PROD1-RED-MED", InStock = 5, Reserved = 0 },
                        new ProductSku { Product = product2, SkuCode = "PROD2-BLUE-MED", InStock = 5, Reserved = 0 },
                        new ProductSku { Product = product3, SkuCode = "PROD3", InStock = 5, Reserved = 0 },
                    };
                }
                else
                {
                    return new List<ProductSku>
                    {
                        new ProductSku { Product = product1, SkuCode = "PROD1-RED-MED", InStock = 5, Reserved = 3 },
                        new ProductSku { Product = product2, SkuCode = "PROD2-BLUE-MED", InStock = 5, Reserved = 2 },
                        new ProductSku { Product = product3, SkuCode = "PROD3", InStock = 5, Reserved = 3 },
                    };
                }
            };
        }

        private Func<string, StateTax> StateTaxFunctionGenerator()
        {
            return (abbr) => new StateTax() { Abbreviation = "IL", Name = "Illinois", TaxRate = 6.5m };
        }

        private Func<int, ShippingMethod> ShippingMethodFunctionGenerator()
        {
            return (id) => new ShippingMethod() { Cost = 9.95m, Description = "UPS Next Day", Id = 3 };
        }
    }
}
