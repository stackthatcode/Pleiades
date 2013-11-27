using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Application.Analytics;
using Commerce.Application.Analytics.Entities;
using Commerce.Application.Database;
using Commerce.Application.Email;
using Commerce.Application.Orders.Entities;
using Commerce.Application.Payment;
using Commerce.Application.Products;

namespace Commerce.Application.Orders
{
    public class OrderManager : IOrderManager
    {
        private readonly PushMarketContext _context;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IAdminEmailBuilder _adminEmailBuilder;
        private readonly ICustomerEmailBuilder _customerEmailBuilder;
        private readonly IEmailService _emailService;
        private readonly IAnalyticsCollector _analyticsCollector;

        public OrderManager(
                PushMarketContext context,  
                Func<IPaymentProcessor> paymentProcessor, 
                IAdminEmailBuilder adminEmailBuilder, 
                ICustomerEmailBuilder customerEmailBuilder, 
                IEmailService emailService, 
                IInventoryRepository inventoryRepository, 
                IAnalyticsCollector analyticsCollector)
        {
            _context = context;
            _adminEmailBuilder = adminEmailBuilder;
            _customerEmailBuilder = customerEmailBuilder;
            _emailService = emailService;
            _inventoryRepository = inventoryRepository;
            _analyticsCollector = analyticsCollector;
            _paymentProcessor = paymentProcessor();
        }

        public List<Order> Find(DateTime? startDate, DateTime? endDate, bool? complete)
        {
            var data = _context.Orders
                .Include(x => x.OrderLines)
                .Include(x => x.Transactions)
                .AsQueryable();

            if (startDate.HasValue)
            {
                data = data.Where(x => x.DateCreated >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                endDate = endDate.Value.AddDays(1);
                data = data.Where(x => x.DateCreated < endDate);
            }
            if (complete.HasValue)
            {
                data = data.Where(x => x.Complete == complete);
            }
            return data
                .OrderByDescending(x =>  x.DateCreated)
                .ToList();
        }

        public Order Retrieve(string externalId)
        {
            var order =
                _context.Orders
                    .Include(x => x.OrderLines)
                    .Include(x => x.StateTax)
                    .Include(x => x.ShippingMethod)
                    .Include(x => x.Transactions)
                    .FirstOrDefault(x => x.ExternalId == externalId);

            order.OrderLines = order.OrderLines.OrderBy(x => x.OriginalSkuCode).ToList();
            return order;
        }

        //
        // TODO: how to prevent non-split Order Lines from interfering with this...?
        //
        public OrderShipment Ship(string externalId, List<int> items)
        {
            var order = this.Retrieve(externalId);
            var actualItems = order.ReadyToShipItems
                .Where(x => items.Contains(x.Id))
                .ToList();

            // This only works on single items!  Should we ever have Quantity in Order Line...? 
            foreach (var item in actualItems)
            {
                var sku = _inventoryRepository.RetreiveBySkuCode(item.OriginalSkuCode);
                if (sku.Reserved >= item.Quantity)
                {
                    item.Status = OrderLineStatus.Shipped;
                    sku.InStock = sku.InStock - item.Quantity;
                    sku.Reserved = sku.Reserved - item.Quantity;
                }
            }
            
            order.LastModified = DateTime.Now;
            order.UpdateComplete();

            var output = new OrderShipment()
                {
                    Order = order,
                    OrderLines = actualItems
                };

            var adminMessage = _adminEmailBuilder.OrderItemsShipped(output);
            var customerMessage = _customerEmailBuilder.OrderItemsShipped(output);
            _emailService.Send(adminMessage);
            _emailService.Send(customerMessage);
            return output;
        }

        public OrderRefund Refund(string externalId, List<int> items)
        {
            var order = this.Retrieve(externalId);
            var currentTotal = order.Total.GrandTotal;

            var refundItems = order.RefundableLines
                .Where(x => items.Contains(x.Id))
                .ToList();

            refundItems.ForEach(x => x.Status = OrderLineStatus.Refunded);
            var postRefundTotal = order.Total.GrandTotal;
            var refundAmount = currentTotal - postRefundTotal;

            if (refundAmount == 0)
            {
                refundItems.ForEach(x => _context.RefreshEntity(x));
                return null;
            }

            var paymentTransaction = order.Payment;
            var result = _paymentProcessor.Refund(paymentTransaction, refundAmount);
            order.Transactions.Add(result);
            var refund = new OrderRefund()
            {
                Order = order,
                OrderLines = refundItems,
                Transaction = result,
            };

            if (!result.Success)
            {
                refundItems.ForEach(x => _context.RefreshEntity(x));
            }
            else
            {
                order.UpdateComplete();
                order.LastModified = DateTime.Now;

                var analyticsRefundItems =
                    refund.OrderLines
                        .ToOrderLineGroups() 
                        .Select(x => new RefundItem
                        {
                            Quantity = x.Quantity,
                            SkuCode = x.SkuCode,
                            UnitPrice = x.Price,
                        })
                        .ToList();
                _analyticsCollector.Refund(DateTime.Now, order.Id, refund.Transaction.Amount, analyticsRefundItems);

                var adminMessage = _adminEmailBuilder.OrderItemsRefunded(refund);
                var customerMessage = _customerEmailBuilder.OrderItemsRefunded(refund);
                _emailService.Send(adminMessage);
                _emailService.Send(customerMessage);
            }

            return refund;
        }

        public void FailShipping(string externalId, List<int> items)
        {
            var order = this.Retrieve(externalId);
            order.ShippedItems
                .Where(x => items.Contains(x.Id))
                .ToList()
                .ForEach(x => x.Status = OrderLineStatus.FailedShipping);
        }

        public void Return(string externalId, List<int> items)
        {
            var order = this.Retrieve(externalId);
            order.OrderLines
                .Where(x => items.Contains(x.Id))
                .ToList()
                .ForEach(x => x.Status = OrderLineStatus.Returned);
        }
    }
}
