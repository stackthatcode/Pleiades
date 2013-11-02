using System;
using System.Web.Mvc;
using Commerce.Application.Interfaces;
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

        public JsonNetResult Sales(string startDate, string endDate, string analytic)
        {
            if (analytic == "TotalSalesAmountsByDate")
            {
                return new JsonNetResult(_analyticsAggregator
                    .TotalSalesAmountsByDate(DateTime.Parse(startDate), DateTime.Parse(endDate)));
            }

            throw new NotImplementedException();
        }

    }
}
