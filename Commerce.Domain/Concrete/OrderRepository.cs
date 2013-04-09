using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Orders;
using Commerce.Persist.Model.Products;

namespace Commerce.Persist.Concrete
{
    public class OrderRepository : IOrderRepository
    {
        PleiadesContext Context { get; set; }
        IPaymentProcessor PaymentProcessor { get; set; }

        public OrderRepository(PleiadesContext context, IPaymentProcessor paymentProcessor)
        {
            this.Context = context;
            this.PaymentProcessor = paymentProcessor;
        }

        public OrderRequestResult SubmitOrder(OrderRequest orderRequest)
        {
            try
            {
                return SubmitOrder_worker(orderRequest);
            }
            catch (Exception ex)
            {
                // TODO: Log it!
                return new OrderRequestResult(false, "Oh noes! Something went wrong while processing your Order");
            }
        }

        public OrderRequestResult SubmitOrder_worker(OrderRequest orderRequest)
        {
            if (orderRequest.BillingInfo == null || orderRequest.ShippingInfo == null ||
                            orderRequest.Items == null || orderRequest.Items.Count() == 0)
            {
                return new OrderRequestResult(false, "Invalid or missing data passed");
            }

            var skus = orderRequest.Items.Select(x => x.SkuCode).ToList();
            var inventory = this.Context.ProductSkus
                    .Include(x => x.Product)
                    .Where(x => skus.Contains(x.SkuCode)).ToList();

            var orderLines =
                orderRequest.Items
                    .Select(x => new OrderLine(inventory.First(y => y.SkuCode == x.SkuCode), x.Quantity))
                    .ToList();

            var stateTax =
                    this.Context.StateTaxes
                        .First(x => x.Abbreviation == orderRequest.BillingInfo.State);

            var shippingMethod =
                    this.Context.ShippingMethods
                        .First(x => x.Id == orderRequest.ShippingInfo.ShippingOptionId);

            var order = new Order()
            {
                EmailAddress = orderRequest.ShippingInfo.EmailAddress,
                Name = orderRequest.ShippingInfo.Name,
                Address1 = orderRequest.ShippingInfo.Address1,
                Address2 = orderRequest.ShippingInfo.Address2,
                City = orderRequest.ShippingInfo.City,
                State = orderRequest.ShippingInfo.State,
                ZipCode = orderRequest.ShippingInfo.ZipCode,
                Phone = orderRequest.ShippingInfo.Phone,

                OrderLines = orderLines,
                StateTax = stateTax,
                ShippingMethod = shippingMethod,
            };

            var paymentResponse = PaymentProcessor.AuthorizeAndCollect(orderRequest.BillingInfo, order.GrandTotal);
            if (paymentResponse.Success == false)
            {
                return new OrderRequestResult(false, "Something's wrong with your Payment Info. Sorry, please try again");
            }

            var orderResponse = new OrderRequestResult() { Success = true };

            // Inventory corrections
            this.Context.RefreshCollection(inventory);
            foreach (var line in order.OrderLines)
            {
                var sku = inventory.First(x => x.SkuCode == line.OriginalSkuCode);
                if (sku.IsDeleted == true || sku.Available < line.Quantity)
                {
                    line.Quantity = sku.Available;
                    orderResponse.Messages.Add(
                        "Only " + sku.Available + " of the " + line.OriginalDescription +
                        " were available in stock.  The size of your order was reduced.");
                }
            }

            // Invoke the Analytics Service - TODO

            return orderResponse;
        }
    }
}
