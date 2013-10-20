using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Application.Database;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Billing;
using Commerce.Application.Model.Orders;
using Commerce.Application.Model.Products;

namespace Commerce.Application.Concrete.Orders
{
    public class OrderSubmissionService : IOrderSubmissionService
    {
        public const string ErrorFault = "Oh noes! Something went wrong while processing your Order";
        public const string ErrorMissingData = "Invalid or missing data passed";
        public const string ErrorFailedPayment = "Something's wrong with your Payment Info. Sorry, please try again";

        PushMarketContext Context { get; set; }
        IPaymentProcessor PaymentProcessor { get; set; }
        IAnalyticsService AnalyticsService { get; set; }
        IEmailService EmailService { get; set; }

        // injectible functions
        public Func<string, StateTax> StateTaxByAbbr;
        public Func<int, ShippingMethod> ShippingMethodById;
        public Func<IEnumerable<string>, bool, List<ProductSku>> InventoryBySkuCodes;
        public Action<SubmitOrderResult> ResponseTesting;
        public Action<Order> CreateOrder;
        public Action SaveChanges;

        public OrderSubmissionService(PushMarketContext context, 
                IPaymentProcessor paymentProcessor, IAnalyticsService analyticsService, IEmailService emailService)
        {
            Context = context;
            PaymentProcessor = paymentProcessor;
            AnalyticsService = analyticsService;
            EmailService = emailService;

            // injectible function initialization
            StateTaxByAbbr = (abbr) => Context.StateTaxes.First(x => x.Abbreviation == abbr);
            ShippingMethodById = (id) => Context.ShippingMethods.First(x => x.Id == id);
            InventoryBySkuCodes = InventoryBySkuCodesImpl;
            CreateOrder = CreateOrderImpl;
            SaveChanges = SaveChangesImpl;
        }

        public SubmitOrderResult Submit(OrderRequest orderRequest)
        {
            try
            {
                return Submit_worker(orderRequest);
            }
            catch (Exception ex)
            {
                // TODO: Log it!
                return new SubmitOrderResult(false, ErrorFault);
            }
        }

        public SubmitOrderResult Submit_worker(OrderRequest orderRequest)
        {
            // Order Request to Order translation - Bounded Context
            if (orderRequest.BillingInfo == null || orderRequest.ShippingInfo == null || 
                orderRequest.Items == null || orderRequest.Items.Count() == 0)
            {
                return new SubmitOrderResult(false, ErrorMissingData);
            }
            var order = FromOrderRequest(orderRequest);

            // Payment Processing - Bounded Context
            var paymentTransaction = PaymentProcessor.AuthorizeAndCollect(orderRequest.BillingInfo, order.Total.GrandTotal);
            if (paymentTransaction.Success == false)
            {
                // Don't create the Order!
                return new SubmitOrderResult(false, ErrorFailedPayment);
            }

            // Payment Received + Create Order - Bounded Context
            order.AddTransaction(paymentTransaction);
            CreateOrder(order);
            
            // Inventory corrections - Bounded Context
            var inventory = InventoryBySkuCodes(order.AllSkuCodes, true);

            foreach (var line in order.OrderLines)
            {
                var sku = inventory.First(x => x.SkuCode == line.OriginalSkuCode);
                if (sku == null || sku.IsDeleted == true)
                {
                    line.Quantity = 0;
                    if (sku.Available == 0 || sku.IsDeleted == true)
                    {
                        order.AddNote(
                            "There are no longer any of the " + line.OriginalName + " available in stock.");
                    }
                }
                else if (sku.Available < line.Quantity)                
                {
                    var wereOrWas = sku.Available == 1 ? "was" : "were";
                    order.AddNote(
                        "Only " + sku.Available + " of the " + line.Quantity + " " + line.OriginalName +
                        " you requested " + wereOrWas + " in stock.  Your order was adjusted.");
                    line.Quantity = sku.Available;
                }

                // Increment the Reserved Count in INVENTORY
                sku.Reserved += line.Quantity;
            }
            SaveChanges();

            // Shipping nothing?  Kill the shipping
            if (order.OrderLines.All(x => x.Quantity == 0))
            {
                order.ShippingMethod = null;
            }

            // Eventual Consistency w/ Payment Correction - Do we need to process a refund? - Bounded Context
            if (order.Total.GrandTotal < order.OriginalGrandTotal)
            {
                var refundAmount = order.OriginalGrandTotal - order.Total.GrandTotal;
                var refundTransaction = PaymentProcessor.Refund(paymentTransaction, refundAmount);
                order.AddTransaction(refundTransaction);    // NOTE: if this failed, it will appear for the Customer
            }

            // Send the Email - Bounded Context
            EmailService.SendOrderReceived();

            // Invoke the Analytics Service - Bounded Context
            AnalyticsService.AddSale(order, 3);

            // FIN! - return OrderRequestResponse - Bounded Context
            var orderResponse = new SubmitOrderResult(order);
            return orderResponse;
        }

        private Order FromOrderRequest(OrderRequest orderRequest)
        {
            var skus = orderRequest.AllSkuCodes;
            var inventory = InventoryBySkuCodes(skus, false);
            
            var orderLines =
                orderRequest.Items
                    .Select(x => new OrderLine(inventory.First(y => y.SkuCode == x.SkuCode), x.Quantity))
                    .ToList();

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
            };

            // Need these to get the right GrandTotal
            var stateTax = StateTaxByAbbr(orderRequest.BillingInfo.State);
            var shippingMethod = ShippingMethodById(orderRequest.ShippingInfo.ShippingOptionId);

            order.ShippingMethod = shippingMethod;
            order.StateTax = stateTax;
            
            order.OriginalGrandTotal = order.Total.GrandTotal;
            order.ExternalId = OrderNumberGenerator.Next();
            order.DateCreated = DateTime.UtcNow;
            return order;
        }

        // Injectible functions
        private List<ProductSku> InventoryBySkuCodesImpl(IEnumerable<string> sku_codes, bool refresh)
        {
            var inventory =
                Context.ProductSkus
                    .Include(x => x.Product)
                    .Include(x => x.Color)
                    .Include(x => x.Size)
                    .Include(x => x.Color.ProductImageBundle)
                    .Where(x => x.IsDeleted == false && sku_codes.Contains(x.SkuCode)).ToList();

            if (refresh)
            {
                Context.RefreshCollection(inventory);
            }
            return inventory;
        }

        private void CreateOrderImpl(Order order)
        {
            Context.Orders.Add(order);
            Context.SaveChanges();
        }

        private void SaveChangesImpl()
        {
            Context.SaveChanges();
        }
    }
}
