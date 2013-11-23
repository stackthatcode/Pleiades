using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Application.Billing;
using Commerce.Application.Database;
using Commerce.Application.Orders.Entities;
using Commerce.Application.Payment;

namespace Commerce.Application.Orders
{
    public class OrderManager : IOrderManager
    {
        private readonly PushMarketContext _context;
        private readonly IPaymentProcessor _paymentProcessor;

        public OrderManager(PushMarketContext context, Func<IPaymentProcessor> paymentProcessor)
        {
            _context = context;
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
            return _context.Orders
                    .Include(x => x.OrderLines)
                    .Include(x => x.StateTax)
                    .Include(x => x.ShippingMethod)
                    .Include(x => x.Transactions)
                    .FirstOrDefault(x => x.ExternalId == externalId);
        }


        public OrderShipment Ship(string externalId, List<int> items)
        {
            var order = this.Retrieve(externalId);
            var actualItems = order.ReadyToShipItems
                .Where(x => items.Contains(x.Id))
                .ToList();

            actualItems.ForEach(x => x.Status = OrderLineStatus.Shipped);
            order.LastModified = DateTime.Now;
            order.UpdateComplete();
            return new OrderShipment()
                {
                    Order = order,
                    OrderLines = actualItems
                };
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
            if (!result.Success)
            {
                refundItems.ForEach(x => _context.RefreshEntity(x));
            }
            else
            {
                order.UpdateComplete();
                order.LastModified = DateTime.Now;  
            }
            return new OrderRefund()
                {
                    Order = order,
                    OrderLines = refundItems,
                    Transaction = result,
                };
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
