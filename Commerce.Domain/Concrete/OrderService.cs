using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Billing;
using Commerce.Persist.Model.Orders;
using Commerce.Persist.Model.Products;

namespace Commerce.Persist.Concrete
{
    public class OrderService : IOrderService
    {
        public const string ErrorFault = "Oh noes! Something went wrong while processing your Order";
        public const string ErrorMissingData = "Invalid or missing data passed";
        public const string ErrorFailedPayment = "Something's wrong with your Payment Info. Sorry, please try again";

        PleiadesContext Context { get; set; }
        IPaymentProcessor PaymentProcessor { get; set; }
        IAnalyticsService AnalyticsService { get; set; }
        IEmailService EmailService { get; set; }

        // injectible functions
        public Func<string, StateTax> StateTaxByAbbr;
        public Func<int, ShippingMethod> ShippingMethodById;
        public Func<IEnumerable<string>, bool, List<ProductSku>> InventoryBySkuCodes;
        public Action<OrderRequestResult> ResponseTesting;
        public Action<Order> CreateOrder;
        public Action SaveChanges;

        public OrderService(PleiadesContext context, 
                IPaymentProcessor paymentProcessor, IAnalyticsService analyticsService, IEmailService emailService)
        {
            this.Context = context;
            this.PaymentProcessor = paymentProcessor;
            this.AnalyticsService = analyticsService;
            this.EmailService = emailService;

            // injectible function initialization
            this.StateTaxByAbbr = (abbr) => this.Context.StateTaxes.First(x => x.Abbreviation == abbr);
            this.ShippingMethodById = (id) => this.Context.ShippingMethods.First(x => x.Id == id);
            this.InventoryBySkuCodes = this.InventoryBySkuCodesImpl;
            this.CreateOrder = this.CreateOrderImpl;
            this.SaveChanges = this.SaveChangesImpl;
        }

        public OrderRequestResult SubmitOrderRequest(OrderRequest orderRequest)
        {
            try
            {
                return SubmitOrder_worker(orderRequest);
            }
            catch (Exception ex)
            {
                // TODO: Log it!
                return new OrderRequestResult(false, ErrorFault);
            }
        }

        public OrderRequestResult SubmitOrder_worker(OrderRequest orderRequest)
        {
            // Order Request to Order translation - Bounded Context
            if (orderRequest.BillingInfo == null || orderRequest.ShippingInfo == null || 
                orderRequest.Items == null || orderRequest.Items.Count() == 0)
            {
                return new OrderRequestResult(false, ErrorMissingData);
            }
            var order = FromOrderRequest(orderRequest);

            // Payment Processing - Bounded Context
            var paymentTransaction = 
                this.PaymentProcessor.AuthorizeAndCollect(orderRequest.BillingInfo, order.GrandTotal);
            if (paymentTransaction.Success == false)
            {
                // Don't create the Order!
                return new OrderRequestResult(false, ErrorFailedPayment);
            }

            // Payment Received + Create Order - Bounded Context
            order.AddTransaction(paymentTransaction);
            this.CreateOrder(order);
            
            // Inventory corrections - Bounded Context
            var inventory = this.InventoryBySkuCodes(order.AllSkuCodes, true);

            foreach (var line in order.OrderLines)
            {
                var sku = inventory.First(x => x.SkuCode == line.OriginalSkuCode);
                if (sku.IsDeleted == true || sku.Available < line.Quantity)
                {
                    line.Quantity = sku.Available;

                    if (sku.Available == 0 || sku.IsDeleted == true)
                    {
                        order.AddNote(
                            "There are no longer any of the " + line.OriginalDescription + " available in stock.");
                    }
                    else
                    {
                        order.AddNote(
                            "Only " + sku.Available + " of the " + line.OriginalDescription +
                            " were available in stock.  The size of your order was reduced.");
                    }
                }
            }
            this.SaveChanges();

            // Eventual Consistency w/ Payment Correction - Do we need to process a refund? - Bounded Context
            if (order.GrandTotal < order.OriginalGrandTotal)
            {
                var refundAmount = order.OriginalGrandTotal - order.GrandTotal;
                var refundTransaction = this.PaymentProcessor.Refund(paymentTransaction, refundAmount);
                order.AddTransaction(refundTransaction);    // NOTE: if this failed, it will appear for the Customer
            }

            // Send the Email - Bounded Context
            this.EmailService.SendOrderReceived();

            // Invoke the Analytics Service - Bounded Context
            this.AnalyticsService.AddSale(order);

            // FIN! - return OrderRequestResponse - Bounded Context
            var orderResponse = new OrderRequestResult(order);
            return orderResponse;
        }

        private Order FromOrderRequest(OrderRequest orderRequest)
        {
            var skus = orderRequest.AllSkuCodes;
            var inventory = this.InventoryBySkuCodes(skus, false);
            
            var orderLines =
                orderRequest.Items
                    .Select(x => new OrderLine(inventory.First(y => y.SkuCode == x.SkuCode), x.Quantity))
                    .ToList();

            var stateTax = this.StateTaxByAbbr(orderRequest.BillingInfo.State);

            var shippingMethod = this.ShippingMethodById(orderRequest.ShippingInfo.ShippingOptionId);

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

        // Injectible functions
        private List<ProductSku> InventoryBySkuCodesImpl(IEnumerable<string> sku_codes, bool refresh)
        {
            var inventory =
                this.Context.ProductSkus
                    .Include(x => x.Product)
                    .Where(x => sku_codes.Contains(x.SkuCode)).ToList();

            if (refresh)
            {
                this.Context.RefreshCollection(inventory);
            }
            return inventory;
        }

        private void CreateOrderImpl(Order order)
        {
            this.Context.Orders.Add(order);
            this.Context.SaveChanges();
        }

        private void SaveChangesImpl()
        {
            this.Context.SaveChanges();
        }
    }
}
