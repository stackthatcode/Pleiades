//using Commerce.Application.Orders.Entities;
//using Commerce.Application.Products;
//using Commerce.Application.Products.Entities;
//using NUnit.Framework;
//using Autofac;
//using Commerce.Application.Email;

//namespace Commerce.IntegrationTests.Infrastructure
//{
//    [TestFixture]
//    public class TestEmailRenderingEngine
//    {
//        private ProductSku _sku1;
//        private ProductSku _sku2;

//        [SetUp]
//        public void SetUp()
//        {
//            _sku1 = new ProductSku
//            {
//                SkuCode = "TATAMI-123",
//                Product = new Product
//                    {
//                        Description = "Tatami Estilo 3.0 Gi",
//                        UnitPrice = 200.00m,
//                    }
//            };
//            _sku2 = new ProductSku
//            {
//                SkuCode = "BULLTERRIER-888",
//                Product = new Product
//                {
//                    Description = "Bull Terrier Mushin Gi",
//                    UnitPrice = 300.00m,
//                },
//            };   
//        }

//        [Test]
//        public void Send_An_Order_Received_Email()
//        {
//            using (var scope = TestContainer.LifetimeScope())
//            {
//                var order = new Order();
//                order.EmailAddress = "aleksjones@gmail.com";
//                order.OrderLines.Add(new OrderLine { Sku = _sku1, OriginalSkuCode =  _sku1.SkuCode, Quantity = 1});
//                order.OrderLines.Add(new OrderLine { Sku = _sku2, OriginalSkuCode = _sku2.SkuCode, Quantity = 2});

//                var builder = scope.Resolve<ICustomerEmailBuilder>();
//                builder.Recipient(order.EmailAddress);
//                builder.ContentCreateOrderReceived();
//                // How do we express different classes of email events...?

//                var service = scope.Resolve<IEmailService>();
//                service.Send(message);
//            }
//        }
//    }
//}
