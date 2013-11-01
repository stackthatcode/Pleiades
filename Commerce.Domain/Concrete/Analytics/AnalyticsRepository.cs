﻿using System;
using System.Collections.Generic;
using Commerce.Application.Database;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Analytics;

namespace Commerce.Application.Concrete.Analytics
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private PushMarketContext _context;

        public AnalyticsRepository(PushMarketContext context)
        {
            _context = context;
        }

        public List<PurchaseOrderEvent> PurchaseOrderEventsByDate(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public List<PurchaseSkuEvent> PurchaseSkuEventsByDate(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public List<RefundSkuEvent> RefundSkuEventsByDate(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public List<RefundEvent> RefundEventsByDate(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
