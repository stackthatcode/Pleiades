using System.Collections.Generic;

namespace Commerce.Application.Model.Billing
{
    public class TransactionType
    {
        public const int AuthorizeAndCollect = 1;
        public const int Refund = 2;
        public const int ChargeBack = 3;
    }

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