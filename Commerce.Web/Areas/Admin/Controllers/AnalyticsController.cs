using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commerce.Application.Analytics;
using Commerce.Application.Analytics.Entities;
using Pleiades.Web.Json;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class AnalyticsController : Controller
    {
        private IAnalyticsAggregator _analyticsAggregator;

        public AnalyticsController(IAnalyticsAggregator analyticsAggregator)
        {
            _analyticsAggregator = analyticsAggregator;
        }

        public JsonNetResult Data(string startDate, string endDate, string analytic)
        {
            if (analytic == "TotalSalesAmountsByDate")
            {
                return MoneyTimeAnalytic(_analyticsAggregator.TotalSalesAmountsByDate, startDate, endDate);
            }
            if (analytic == "TotalSalesQuantitiesByDate")
            {
                return MoneyTimeAnalytic(_analyticsAggregator.TotalSalesQuantitiesByDate, startDate, endDate);
            }
            if (analytic == "TotalRefundQuantitiesByDate")
            {
                return MoneyTimeAnalytic(_analyticsAggregator.TotalRefundQuantitiesByDate, startDate, endDate);
            }
            if (analytic == "TotalRefundAmountsByDate")
            {
                return MoneyTimeAnalytic(_analyticsAggregator.TotalRefundAmountsByDate, startDate, endDate);
            }

            if (analytic == "TotalSalesAmountsBySku")
            {
                return SkuTotalAnalytic(_analyticsAggregator.TotalSalesAmountsBySku, startDate, endDate);
            }
            if (analytic == "TotalSalesQuantityBySku")
            {
                return SkuTotalAnalytic(_analyticsAggregator.TotalSalesQuantityBySku, startDate, endDate);
            }
            if (analytic == "TotalRefundAmountsBySku")
            {
                return SkuTotalAnalytic(_analyticsAggregator.TotalRefundAmountsBySku, startDate, endDate);
            }
            if (analytic == "TotalRefundQuantityBySku")
            {
                return SkuTotalAnalytic(_analyticsAggregator.TotalRefundQuantityBySku, startDate, endDate);
            }

            throw new NotImplementedException();
        }

        private JsonNetResult MoneyTimeAnalytic(Func<DateTime, DateTime, List<DateTotal>> analytic, string startDate, string endDate)
        {
            var data = analytic(DateTime.Parse(startDate), DateTime.Parse(endDate));
            var output = data.Select(x => new[] { x.DateTime.ToFlotTime(), (double)x.Amount }).ToList();
            return new JsonNetResult(output);
        }

        private JsonNetResult SkuTotalAnalytic(Func<DateTime, DateTime, List<SkuTotal>> analytic, string startDate, string endDate, int take = 5)
        {
            var data = analytic(DateTime.Parse(startDate), DateTime.Parse(endDate));
            var output = data.Take(take).Select(x => new { label = x.SkuCode, data = x.Amount }).ToList();
            return new JsonNetResult(output);            
        }
    }
    
    public static class LilDataTimeExtensions
    {
        public static double ToFlotTime(this DateTime input)
        {
            return input
               .Subtract(new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc))
               .TotalMilliseconds;
        }
    }
}
