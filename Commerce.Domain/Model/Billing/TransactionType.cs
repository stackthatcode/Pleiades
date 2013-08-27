using System.Collections.Generic;

namespace Commerce.Application.Model.Billing
{
    public enum TransactionType
    {
        AuthorizeAndCollect = 1,
        Refund = 2,
        ChargeBack = 3,
    }

    public static class TransactionTypeExtensions
    {
        private readonly static 
            Dictionary<TransactionType, string> _descriptions =
                new Dictionary<TransactionType, string>()
                {
                    { TransactionType.AuthorizeAndCollect, "Payment" },
                    { TransactionType.Refund, "Refund" },
                    { TransactionType.ChargeBack, "Charge Back" },
                };

        public static string ToPlainEnglish(this TransactionType transactionType)
        {
            return _descriptions[transactionType];
        }
    }
}