using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Analytics.Entities;
using Commerce.Application.Database;

namespace Commerce.Application.Analytics
{
    public class AnalyticsAggregator : IAnalyticsAggregator
    {
        private readonly PushMarketContext _context;

        public AnalyticsAggregator(PushMarketContext context)
        {
            _context = context;
        }

        public List<DateTotal> TotalSalesAmountsByDate(DateTime @from, DateTime to)
        {
            return AggregatePurchaseOrderEvents(@from, @to, x => x.SaleAmount)
                .FillEmptyDates(from, to);
        }

        public List<DateTotal> TotalSalesQuantitiesByDate(DateTime @from, DateTime to)
        {
            return AggregatePurchaseOrderEvents(@from, @to, x => x.Quantity)
                .FillEmptyDates(from, to);
        }

        public List<DateTotal> TotalRefundAmountsByDate(DateTime @from, DateTime to)
        {
            return AggregateRefundEvents(@from, to, x => x.Amount)
                .FillEmptyDates(from, to);
        }

        public List<DateTotal> TotalRefundQuantitiesByDate(DateTime @from, DateTime to)
        {
            return AggregateRefundEvents(@from, to, x => x.Quantity)
                .FillEmptyDates(from, to);
        }


        public List<SkuTotal> TotalSalesAmountsBySku(DateTime @from, DateTime to)
        {
            var data = _context.PurchaseSkuEvents.Where(x => x.Date >= from && x.Date <= to);
            return data
                .Select(x => x.SkuCode)
                .Distinct()
                .Select(x => new SkuTotal
                    {
                        SkuCode = x,
                        Amount = data.Where(@event => @event.SkuCode == x).Sum(@event => @event.Amount)
                    })
                .OrderBy(x => x.Amount)
                .ToList();
        }

        public List<SkuTotal> TotalSalesQuantityBySku(DateTime @from, DateTime to)
        {
            var data = _context.PurchaseSkuEvents.Where(x => x.Date >= from && x.Date <= to);
            return data
                .Select(x => x.SkuCode)
                .Distinct()
                .Select(x => new SkuTotal
                {
                    SkuCode = x,
                    Amount = data.Where(@event => @event.SkuCode == x).Sum(@event => @event.Quantity)
                })
                .OrderBy(x => x.Amount)
                .ToList();            
        }

        public List<SkuTotal> TotalRefundAmountsBySku(DateTime @from, DateTime to)
        {
            var data = _context.RefundSkuEvents.Where(x => x.Date >= from && x.Date <= to);
            return data
                .Select(x => x.SkuCode)
                .Distinct()
                .Select(x => new SkuTotal
                {
                    SkuCode = x,
                    Amount = data
                        .Where(@event => @event.SkuCode == x)
                        .Sum(@event => @event.Amount)
                })
                .OrderBy(x => x.Amount)
                .ToList();            
        }

        public List<SkuTotal> TotalRefundQuantityBySku(DateTime @from, DateTime to)
        {
            var data = _context.RefundSkuEvents.Where(x => x.Date >= from && x.Date <= to);
            return data
                .Select(x => x.SkuCode)
                .Distinct()
                .Select(x => new SkuTotal
                {
                    SkuCode = x,
                    Amount = data
                        .Where(@event => @event.SkuCode == x)
                        .Sum(@event => @event.Quantity)
                })
                .OrderBy(x => x.Amount)
                .ToList();            
        }


        public List<SkuDateTotal> TotalSalesAmountsByDateAndSku(DateTime @from, DateTime to)
        {
            return AggregatePurchaseSkuEvents(@from, to, x => x.Amount);
        }

        public List<SkuDateTotal> TotalSalesQuantitiesByDateAndSku(DateTime @from, DateTime to)
        {
            return AggregatePurchaseSkuEvents(@from, to, x => x.Quantity);
        }

        public List<SkuDateTotal> TotalRefundAmountsByDateAndSku(DateTime @from, DateTime to)
        {
            return AggregateRefundSkuEvents(@from, to, x => x.Amount);
        }

        public List<SkuDateTotal> TotalRefundQuantitiesByDateAndSku(DateTime @from, DateTime to)
        {
            return AggregateRefundSkuEvents(@from, to, x => x.Quantity);
        }


        private List<DateTotal> AggregatePurchaseOrderEvents(
                DateTime @from, DateTime to, Func<PurchaseOrderEvent, decimal> property)
        {
            var events = _context.PurchaseOrderEvents.Where(x => x.Date >= @from && x.Date <= to);
            var uniqueDates = events
                .Select(x => x.Date)
                .Distinct()
                .Select(x => new DateTotal { DateTime = x })
                .ToList();

            uniqueDates
                .ForEach(x => x.Amount = events.Where(e => e.Date == x.DateTime)
                .Sum(property));
            return uniqueDates.ToList();
        }

        private List<DateTotal> AggregateRefundEvents(DateTime @from, DateTime to, Func<RefundEvent, decimal> property)
        {
            var events = _context.RefundEvents.Where(x => x.Date >= @from && x.Date <= to);
            var uniqueDates = events
                .Select(x => x.Date)
                .Distinct()
                .Select(x => new DateTotal { DateTime = x })
                .ToList();

            uniqueDates
                .ForEach(date => date.Amount = events.Where(e => e.Date == date.DateTime).Sum(property));
            return uniqueDates.ToList();
        }

        private List<SkuDateTotal> AggregatePurchaseSkuEvents(DateTime @from, DateTime to, Func<PurchaseSkuEvent, decimal> property)
        {
            var events = _context.PurchaseSkuEvents.Where(x => x.Date >= from && x.Date <= to);
            var distinctSkus = events.Select(x => x.SkuCode).Distinct();
            var dateTotals = events
                .Select(x => x.Date)
                .Distinct()
                .Select(x => new SkuDateTotal { DateTime = x })
                .ToList();

            foreach (var date in dateTotals)
            {
                foreach (var sku in distinctSkus)
                {
                    var total = events.Where(x => x.SkuCode == sku).Sum(property);
                    date.SkuTotals.Add(
                        new SkuTotal()
                        {
                            Amount = total,
                            SkuCode = sku,
                        });
                }
            }
            return dateTotals.ToList();
        }

        private List<SkuDateTotal> AggregateRefundSkuEvents(DateTime @from, DateTime to, Func<RefundSkuEvent, decimal> property)
        {
            var events = _context.RefundSkuEvents.Where(x => x.Date >= @from && x.Date <= to);
            var distinctSkus = events.Select(x => x.SkuCode).Distinct();
            var dateTotals = events
                .Select(x => x.Date)
                .Distinct()
                .Select(x => new SkuDateTotal {DateTime = x})
                .ToList();

            foreach (var date in dateTotals)
            {
                foreach (var sku in distinctSkus)
                {
                    var total = events.Where(x => x.SkuCode == sku).Sum(property);
                    date.SkuTotals.Add(
                        new SkuTotal()
                            {
                                Amount = total,
                                SkuCode = sku,
                            });
                }
            }
            return dateTotals.ToList();
        }
    }

    public static class AggregatorHelperExtensions
    {
        public static List<DateTotal> FillEmptyDates(this List<DateTotal> totals, DateTime @from, DateTime to)
        {
            for (var current = @from; current <= to; current = current.AddDays(1))
            {
                if (totals.All(x => x.DateTime != current))
                {
                    totals.Add(new DateTotal { DateTime = current, Amount = 0 });
                }
            }
            return totals.OrderBy(x => x.DateTime).ToList();
        }        
    }
}
