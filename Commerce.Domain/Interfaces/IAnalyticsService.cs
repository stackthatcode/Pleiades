using System;

namespace Commerce.Persist.Interfaces
{
    public interface IAnalyticsService
    {
        void AddSale(DateTime transactionDate, decimal transactionAmount);
    }
}