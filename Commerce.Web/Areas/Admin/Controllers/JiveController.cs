using System.Web.Mvc;
using System.Linq;
using Commerce.Application.Concrete;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Orders;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class JiveController : Controller
    {
        IAnalyticsService AnalyticsService { get; set; }

        public JiveController(IAnalyticsService analyticsService)
        {
            this.AnalyticsService = analyticsService;
        }

        public override string ToString()
        {
            return string.Format("AnalyticsService: {0}", AnalyticsService);
        }

        //
        // GET: /Admin/Jive/

        public int Compute(int x)
        {
            var result = 7 + 8 + x;

            return x + 3;
        }

        public ActionResult Index(int specialSauce)
        {
            var nextService = new AnalyticService();
            var z = specialSauce;
            var y = AnotherComputer(z);
            const int myVariable = 7;
            var g = Compute(myVariable);
            this.AnalyticsService.AddSale(new Order(), 3);
            return View();
        }

        private static int AnotherComputer(int z)
        {
            return z + 3;
        }
    }
}
