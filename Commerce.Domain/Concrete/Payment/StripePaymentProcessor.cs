using System;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Billing;
using Stripe;

namespace Commerce.Application.Concrete.Payment
{
    public class StripePaymentProcessor : IPaymentProcessor
    {
        private readonly IConfigurationAdapter _configurationAdapter;
        private readonly StripeChargeService _stripeService;

        public StripePaymentProcessor(IConfigurationAdapter configurationAdapter, StripeChargeService stripeService)
        {
            _configurationAdapter = configurationAdapter;
            _stripeService = stripeService;
        }

        public Transaction Charge(string token, decimal amount)
        {
            try
            {
                var result =
                    _stripeService.Create(
                        new StripeChargeCreateOptions
                            {
                                AmountInCents = (int) (amount*100),
                                Card = token,
                                Currency = "usd",
                            }
                        );
                return new Transaction(TransactionType.AuthorizeAndCollect)
                    {
                        Amount = amount,
                        OriginalReferenceCode = token,
                        Success = true,
                        ProcessorResponse = "",
                        Details = "Payment Received - Invoice# " + result.InvoiceId
                    };
            }
            catch (Exception ex)
            {
                return new Transaction(TransactionType.AuthorizeAndCollect)
                {
                    Amount = amount,
                    OriginalReferenceCode = token,
                    Success = false,
                    ProcessorResponse = "",
                    Details = "Payment Failure : " + ex.Message
                };                
            }
        }

        public Transaction Refund(Transaction originalTransaction, decimal amount)
        {
            var result = _stripeService.Refund(originalTransaction.OriginalReferenceCode, (int)(100*amount));
            return new Transaction(TransactionType.Refund)
                {
                    Amount = amount,
                    ProcessorResponse = "",
                    ReferenceCode = result.Id,
                    OriginalReferenceCode = originalTransaction.ReferenceCode,
                };
        }
    }
}
