using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Database;
using Commerce.Application.Model.Orders;

namespace Commerce.Application.Interfaces
{
    public class OrderManager : IOrderManager
    {
        private readonly PushMarketContext _context;
        private readonly IPaymentProcessor _paymentProcessor;

        public OrderManager(PushMarketContext context, IPaymentProcessor paymentProcessor)
        {
            _context = context;
            _paymentProcessor = paymentProcessor;
        }

        public List<Order> Find(DateTime? startDate, DateTime? endDate, bool? complete)
        {
            var data = _context.Orders.AsQueryable();
            if (startDate.HasValue)
                data = data.Where(x => x.DateCreated >= startDate.Value);
            if (endDate.HasValue)
                data = data.Where(x => x.DateCreated >= endDate.Value);
            if (complete.HasValue)
                data = data.Where(x => x.Complete == complete);
            return data.ToList();
        }

        public Order Retrieve(string externalId)
        {
            return _context.Orders.FirstOrDefault(x => x.ExternalId == externalId);
        }

        public void Refund(string externalId, List<int> items)
        {
            var order = this.Retrieve(externalId);
            var currentTotal = order.Total.GrandTotal;

            var refundableItems = order.RefundableLines
                .Where(x => items.Contains(x.Id))
                .ToList();
            refundableItems
                .ForEach(x =>x.Status = OrderLineStatus.Refunded);

            var postRefundTotal = order.Total.GrandTotal;
            var refundAmount = currentTotal - postRefundTotal;
            if (refundAmount == 0)
            {
                return;
            }

            var paymentTransaction = order.Payment;
            _paymentProcessor.Refund(paymentTransaction, refundAmount);
        }

        public void Ship(string externalId, List<int> items)
        {
            var order = this.Retrieve(externalId);
            order.ReadyToShipItems
                .Where(x => items.Contains(x.Id))
                .ToList()
                .ForEach(x => x.Status = OrderLineStatus.Shipped);
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
