using System;
using System.Collections.Generic;
using System.Configuration;
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
        IAnalyticsService AnalyticsService { get; set; }
        IEmailGenerator EmailGenerator { get; set; }
        IEmailService EmailService { get; set; }

        public OrderRepository(
                PleiadesContext context, IPaymentProcessor paymentProcessor, IAnalyticsService analyticsService, 
                IEmailGenerator emailGenerator, IEmailService emailService)
        {
            this.Context = context;
            this.PaymentProcessor = paymentProcessor;
            this.AnalyticsService = analyticsService;
            this.EmailGenerator = emailGenerator;
            this.EmailService = emailService;
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
            // Order Request to Order translation
            if (orderRequest.BillingInfo == null || orderRequest.ShippingInfo == null ||
                            orderRequest.Items == null || orderRequest.Items.Count() == 0)
            {
                return new OrderRequestResult(false, "Invalid or missing data passed");
            }
            var order = FromOrderRequest(orderRequest);

            // Payment Processing
            var paymentTransaction = PaymentProcessor.AuthorizeAndCollect(orderRequest.BillingInfo, order.GrandTotal);
            if (paymentTransaction.Success == false)
            {
                return new OrderRequestResult(false, "Something's wrong with your Payment Info. Sorry, please try again");
            }
            order.PaymentTransactions.Add(paymentTransaction);
            this.Context.Orders.Add(order);
            this.Context.Transactions.Add(paymentTransaction);
            this.Context.SaveChanges();

            // Inventory corrections
            var orderResponse = new OrderRequestResult() { Success = true };
            var inventory = this.InventoryBySkuCodes(order.AllSkuCodes);
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

            // Create the Order
            this.Context.SaveChanges();

            // Send the Email
            var message = this.EmailGenerator.OrderReceived();
            this.EmailService.Send(message);

            // Invoke the Analytics Service
            this.AnalyticsService.AddSale(order);
            this.Context.SaveChanges();

            // ** Temporary Testing hook ** //
            this.ResponseTesting(orderResponse);

            // FIN!
            return orderResponse;
        }

        private Order FromOrderRequest(OrderRequest orderRequest)
        {
            var skus = orderRequest.AllSkuCodes;

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

            order.OriginalGrandTotal = order.GrandTotal;
            return order;
        }

        public List<ProductSku> InventoryBySkuCodes(IEnumerable<string> skuCodes)
        {
            var inventory = this.Context.ProductSkus
                   .Include(x => x.Product)
                   .Where(x => skuCodes.Contains(x.SkuCode)).ToList();
            this.Context.RefreshCollection(inventory);
            return inventory;
        }

        private void ResponseTesting(OrderRequestResult result)
        {
            var OrderSimulateException = Boolean.Parse(ConfigurationManager.AppSettings["OrderSimulateException"]);
            var OrderSimulateFailure = Boolean.Parse(ConfigurationManager.AppSettings["OrderSimulateFailure"]);
            var OrderMessages = ConfigurationManager.AppSettings["OrderMessages"].Split('|').ToList();

            if (OrderSimulateException)
            {
                throw new Exception("Oh noes! Total system failure!!!");
            }
            if (OrderSimulateFailure)
            {
                result.Success = false;
            }
            OrderMessages.ForEach(x => result.Messages.Add(x));
        }
    }
}
