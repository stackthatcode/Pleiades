using System;
using Commerce.Application.Billing;
using Pleiades.Application.Logging;
using Stripe;

namespace Commerce.Application.Payment
{
    public class StripePaymentProcessor : IPaymentProcessor
    {
        private readonly IStripeConfigAdapter _stripeConfigAdapter;
        private readonly StripeChargeService _stripeService;

        public StripePaymentProcessor(IStripeConfigAdapter stripeConfigAdapter, StripeChargeService stripeService)
        {
            _stripeConfigAdapter = stripeConfigAdapter;
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
                        OriginalReferenceCode = result.Id,                        
                        ReferenceCode = result.Id,
                        Success = true,
                        ProcessorResponse = "",
                        Details = "Charged Token# " + token
                    };
            }
            catch (Exception ex)
            {
                LoggerSingleton.Get().Error(ex);
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
            try
            {
                var result = _stripeService.Refund(originalTransaction.OriginalReferenceCode, (int) (100*amount));
                return new Transaction(TransactionType.Refund)
                    {
                        Amount = amount,
                        ProcessorResponse = "",
                        ReferenceCode = result.Id,
                        Success = true,
                        OriginalReferenceCode = originalTransaction.OriginalReferenceCode,
                        Details = "Refund",
                    };
            }
            catch (Exception ex)
            {
                LoggerSingleton.Get().Error(ex);
                return new Transaction(TransactionType.Refund)
                {
                    Amount = amount,
                    ProcessorResponse = "",
                    Success = false,
                    OriginalReferenceCode = originalTransaction.OriginalReferenceCode,
                    Details = "Refund Failure : " + ex.Message
                };
            }
        }
    }
}
