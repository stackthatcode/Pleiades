using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Commerce.Application.Billing;
using Commerce.Application.Orders.Entities;
using Commerce.Application.Products;
using Commerce.Application.Products.Entities;
using NUnit.Framework;
using Autofac;
using Commerce.Application.Email;

namespace Commerce.IntegrationTests.Infrastructure
{
    [TestFixture]
    public class TestEmailTemplates
    {
        private ProductSku _sku1;
        private ProductSku _sku2;

        [SetUp]
        public void SetUp()
        {
            _sku1 = new ProductSku
            {
                SkuCode = "TATAMI-123",
                Product = new Product
                    {
                        Name = "Tatami Estilo 3.0 Gi",
                        UnitPrice = 200.00m,
                    }
            };
            _sku2 = new ProductSku
            {
                SkuCode = "BULLTERRIER-888",
                Product = new Product
                    {
                        Name = "Bull Terrier Mushin Gi",
                        UnitPrice = 300.00m,
                    },
            };
        }

        private Order OrderFactory()
        {
            var order = new Order();
            order.ExternalId = "ARQ-23289";
            order.Name = "Lucious Tucious";
            order.EmailAddress = "aleksjones@gmail.com, jeremysimon@me.com"; 
            order.Phone = "510-717-8112";
            order.Address1 = "123 Test STreet";
            order.Address2 = "Suite 300";
            order.City = "Urbana";
            order.State = "IL";
            order.ZipCode = "60601";
            order.OrderLines.Add(new OrderLine
            {
                Sku = _sku1,
                OriginalSkuCode = _sku1.SkuCode,
                Quantity = 1,
                OriginalName = _sku1.Name,
                OriginalUnitPrice = _sku1.Product.UnitPrice,
            });
            order.OrderLines.Add(new OrderLine
            {
                Sku = _sku2,
                OriginalSkuCode = _sku2.SkuCode,
                Quantity = 2,
                OriginalName = _sku2.Name,
                OriginalUnitPrice = _sku1.Product.UnitPrice,
            });
            return order;
        }

        [Test]
        public void Send_An_Order_Received_Email_To_Customer()
        {
            using (var scope = TestContainer.LifetimeScope())
            {
                var order = OrderFactory();
                var builder = scope.Resolve<ICustomerEmailBuilder>();
                var message = builder.OrderReceived(order);
                var service = scope.Resolve<IEmailService>();
                service.Send(message);
            }
        }

        [Test]
        public void Send_An_Order_Items_Shipped_Email_To_Customer()
        {
            using (var scope = TestContainer.LifetimeScope())
            {
                var order = OrderFactory();
                var shipment = new OrderShipment()
                {
                    Order = order,
                    OrderLines = new[] { order.OrderLines[0] }.ToList()
                };

                var builder = scope.Resolve<ICustomerEmailBuilder>();
                
                var message = builder.OrderItemsShipped(shipment);
                var service = scope.Resolve<IEmailService>();
                service.Send(message);
            }
        }

        [Test]
        public void Send_An_Order_Refund_Email_To_Customer()
        {
            using (var scope = TestContainer.LifetimeScope())
            {
                var order = OrderFactory();
                var refund = new OrderRefund()
                {
                    Order = order,
                    OrderLines = new[] { order.OrderLines[0] }.ToList(),
                    Transaction = new Transaction(TransactionType.Refund)
                        {
                            Amount = 120m,
                        }
                };

                var builder = scope.Resolve<ICustomerEmailBuilder>();
                var message = builder.OrderItemsRefunded(refund);
                var service = scope.Resolve<IEmailService>();
                service.Send(message);
            }
        }

        private const string AdminMailingList = "aleksjones@gmail.com, jeremysimon@me.com";

        [Test]
        public void Send_An_Order_Received_Email_To_Admin()
        {
            using (var scope = TestContainer.LifetimeScope())
            {
                var order = OrderFactory();
                var builder = scope.Resolve<IAdminEmailBuilder>();
                var message = builder.OrderReceived(order);
                message.To = AdminMailingList;
                var service = scope.Resolve<IEmailService>();
                service.Send(message);
            }
        }

        [Test]
        public void Send_An_Order_Items_Shipped_Email_To_Admin()
        {
            using (var scope = TestContainer.LifetimeScope())
            {
                var order = OrderFactory();
                var shipment = new OrderShipment()
                {
                    Order = order,
                    OrderLines = new[] { order.OrderLines[0] }.ToList()
                };

                var builder = scope.Resolve<IAdminEmailBuilder>();
                var message = builder.OrderItemsShipped(shipment);
                message.To = AdminMailingList;

                var service = scope.Resolve<IEmailService>();
                service.Send(message);
            }
        }

        [Test]
        public void Send_An_Order_Refund_Email_To_Admin()
        {
            using (var scope = TestContainer.LifetimeScope())
            {
                var order = OrderFactory();
                var refund = new OrderRefund()
                {
                    Order = order,
                    OrderLines = new[] { order.OrderLines[0] }.ToList(),
                    Transaction = new Transaction(TransactionType.Refund)
                    {
                        Amount = 120m,
                    }
                };

                var builder = scope.Resolve<IAdminEmailBuilder>();
                var message = builder.OrderItemsRefunded(refund);
                message.To = AdminMailingList;
                var service = scope.Resolve<IEmailService>();
                service.Send(message);
            }
        }

        [Test]
        public void Send_An_SystemError_Notification_Email_To_Admin()
        {
            using (var scope = TestContainer.LifetimeScope())
            {               
                try
                {
                    throw new Exception("Oh noes! I'm melting!");
                }
                catch (Exception ex)
                {
                    var builder = scope.Resolve<IAdminEmailBuilder>();
                    var message = builder.SystemError(Guid.NewGuid(), ex);
                    message.To = AdminMailingList;
                    var service = scope.Resolve<IEmailService>();
                    service.Send(message);                    
                }
            }
        }

        [Test]
        [Explicit]
        public void Send_And_Email()
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("artofgroundfighting@gmail.com", "qVk8juBpNsFpd3MY9Ovu"),
                EnableSsl = true
            };
            client.Send("artofgroundfighting@gmail.com", "artofgroundfighting@gmail.com", "test", "testbody");
        }
    }
}
