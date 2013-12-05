using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Application.Analytics;
using Commerce.Application.Billing;
using Commerce.Application.Database;
using Commerce.Application.Email;
using Commerce.Application.Orders.Entities;
using Commerce.Application.Payment;
using Commerce.Application.Products;
using Pleiades.App.Logging;

namespace Commerce.Application.Orders
{
    public class OrderService : IOrderService
    {
        public const string ErrorFault = "Oh noes! Something went wrong while processing your Order";
        public const string ErrorMissingData = "Invalid or missing data passed";
        public const string ErrorFailedPayment = "Something's wrong with your Payment Info. Sorry, please try again";

        private readonly PushMarketContext _context;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IAnalyticsCollector _analyticsService;
        private readonly IAdminEmailBuilder _adminEmailBuilder;
        private readonly ICustomerEmailBuilder _customerEmailBuilder;
        private readonly IEmailService _emailService;
    
        // Post Processing Pipline functions
        public Action<Order> AddOrderToDatabase;
        public Action<Order> InventoryCorrection;
        public Action<Order> RefundDifference;
        public Action SaveChangesToDatabase;
        public Action<Order> EmailNotification;
        public Action<Order> PublishToAnalytics;
        public Action<Order> SplitOrderLine;

        // Dependencies
        public Func<string, StateTax> StateTaxByAbbr;
        public Func<int, ShippingMethod> ShippingMethodById;
        public Action<SubmitOrderResult> ResponseTesting;
        public Func<IEnumerable<string>, bool, List<ProductSku>> InventoryBySkuCodes;
        
        public OrderService(PushMarketContext context, 
                Func<IPaymentProcessor> paymentProcessorFactory, 
                IAnalyticsCollector analyticsService, 
                IEmailService emailService, 
                IAdminEmailBuilder adminEmailBuilder, 
                ICustomerEmailBuilder customerEmailBuilder)
        {
            _context = context;
            _paymentProcessor = paymentProcessorFactory.Invoke();
            _analyticsService = analyticsService;
            _emailService = emailService;
            _adminEmailBuilder = adminEmailBuilder;
            _customerEmailBuilder = customerEmailBuilder;

            // injectible function initialization
            StateTaxByAbbr = (abbr) => _context.StateTaxes.First(x => x.Abbreviation == abbr);
            ShippingMethodById = (id) => _context.ShippingMethods.First(x => x.Id == id);
            InventoryBySkuCodes = InventoryBySkuCodesImpl;

            // Initialize Order Post-processors
            AddOrderToDatabase = AddOrderToDatabaseImpl;
            InventoryCorrection = InventoryCorrectionImpl;
            RefundDifference = RefundDifferenceImpl;
            SaveChangesToDatabase = SaveChangesToDatabaseImpl;
            EmailNotification = EmailNotificationImpl;
            PublishToAnalytics = PublishToAnalyticsImpl;
            SplitOrderLine = SplitOrderLineImpl;
        }

        public SubmitOrderResult Submit(OrderRequest orderRequest)
        {
            try
            {
                return Submit_worker(orderRequest);
            }
            catch (Exception ex)
            {
                LoggerSingleton.Get().Error(ex);
                return new SubmitOrderResult(false, ErrorFault);
            }
        }

        public Order Retreive(string externalId)
        {
            return this._context.Orders.FirstOrDefault(x => x.ExternalId == externalId);
        }

        public SubmitOrderResult Submit_worker(OrderRequest orderRequest)
        {
            // Order Request to Order translation - Bounded Context
            if (orderRequest.Token == null || orderRequest.ShippingInfo == null || 
                orderRequest.Items == null || !orderRequest.Items.Any())
            {
                return new SubmitOrderResult(false, ErrorMissingData);
            }
            var order = FromOrderRequest(orderRequest);

            // Payment Processing - Bounded Context
            var paymentTransaction = _paymentProcessor.Charge(orderRequest.Token, order.Total.GrandTotal);
            if (paymentTransaction.Success == false)
            {
                // Don't create the Order!
                return new SubmitOrderResult(false, ErrorFailedPayment);
            }
            order.AddTransaction(paymentTransaction);

            // Add the Order to the Database Context
            AddOrderToDatabase(order);
            
            // Inventory corrections - Bounded Context
            InventoryCorrection(order);

            // TODO: research using an Auth / Charge pattern, here
            // If the inventory has changed, then we have to Refund
            RefundDifference(order);

            // Shipping nothing?  Kill the shipping
            RemoveShippingOnEmptyOrder(order);

            // Send the Email - Bounded Context
            EmailNotification(order);

            // Invoke the Analytics Service - Bounded Context
            PublishToAnalytics(order);

            // Last step, Split the Order Lines
            SplitOrderLine(order);

            // FIN! - return OrderRequestResponse - Bounded Context
            return new SubmitOrderResult(order);
        }

        // TODO: create a Composite of Order post-processors
        private void AddOrderToDatabaseImpl(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        private void InventoryCorrectionImpl(Order order)
        {
            var inventory = InventoryBySkuCodes(order.AllSkuCodes, true);

            foreach (var line in order.OrderLines)
            {
                var sku = inventory.First(x => x.SkuCode == line.OriginalSkuCode);
                if (sku == null || sku.IsDeleted)
                {
                    line.Quantity = 0;
                    if (sku.Available == 0 || sku.IsDeleted)
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
            SaveChangesToDatabase();
        }

        private void RemoveShippingOnEmptyOrder(Order order)
        {
            if (order.OrderLines.All(x => x.Quantity == 0))
            {
                order.ShippingMethod = null;
            }
        }

        private void RefundDifferenceImpl(Order order)
        {
            // Eventual Consistency w/ Payment Correction - Do we need to process a refund? - Bounded Context
            if (order.Total.GrandTotal < order.OriginalGrandTotal)
            {
                var originalPayment = order.Transactions.First(x => x.TransactionType == TransactionType.AuthorizeAndCollect);
                var refundAmount = order.OriginalGrandTotal - order.Total.GrandTotal;
                var refundTransaction = _paymentProcessor.Refund(originalPayment, refundAmount);
                order.AddTransaction(refundTransaction); // NOTE: if this failed, it will appear for the Customer
            }
        }

        private void SaveChangesToDatabaseImpl()
        {
            _context.SaveChanges();
        }

        private void EmailNotificationImpl(Order order)
        {
            try
            {
                var customerMessage = _customerEmailBuilder.OrderReceived(order);
                _emailService.Send(customerMessage);

                var adminMessage = _adminEmailBuilder.OrderReceived(order);
                _emailService.Send(adminMessage);
            }
            catch (Exception ex)
            {
                LoggerSingleton.Get().Error(ex);
            } 
        }

        private void PublishToAnalyticsImpl(Order order)
        {
            try
            {
                _analyticsService.Sale(order);
            }
            catch(Exception ex)
            {
                LoggerSingleton.Get().Error(ex);
            }
        }

        private void SplitOrderLineImpl(Order order)
        {
            order.SplitLines();
            SaveChangesToDatabase();
        }
            
        // NOTE: can't this be moved into a Repository...?
        private List<ProductSku> InventoryBySkuCodesImpl(IEnumerable<string> sku_codes, bool refresh)
        {
            var inventory =
                _context.ProductSkus
                    .Include(x => x.Product)
                    .Include(x => x.Color)
                    .Include(x => x.Size)
                    .Include(x => x.Color.ProductImageBundle)
                    .Where(x => x.IsDeleted == false && sku_codes.Contains(x.SkuCode)).ToList();

            if (refresh)
            {
                _context.RefreshCollection(inventory);
            }
            return inventory;
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
            var stateTax = StateTaxByAbbr(orderRequest.ShippingInfo.State);
            var shippingMethod = ShippingMethodById(orderRequest.ShippingInfo.ShippingMethodId);

            order.ShippingMethod = shippingMethod;
            order.StateTax = stateTax;

            order.OriginalGrandTotal = order.Total.GrandTotal;
            order.ExternalId = OrderNumberGenerator.Next();
            order.DateCreated = DateTime.Now;
            return order;
        }
    }
}
