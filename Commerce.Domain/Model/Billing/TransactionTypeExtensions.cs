using System.Collections.Generic;

namespace Commerce.Application.Model.Billing
{
    public static class TransactionTypeExtensions
    {
        private readonly static 
            Dictionary<int, string> _descriptions =
                new Dictionary<int, string>()
                    {
                        { TransactionType.AuthorizeAndCollect, "Payment" },
                        { TransactionType.Refund, "Refund" },
                        { TransactionType.ChargeBack, "Charge Back" },  // How will we capture these...?
                    };

        public static string ToPlainEnglish(this int transactionType)
        {
            return _descriptions[transactionType];
        }
    }
}