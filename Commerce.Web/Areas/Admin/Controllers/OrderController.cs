using System.Linq;
using System.Web.Mvc;
using Commerce.Application.Concrete.Products;
using Commerce.Application.Database;
using Commerce.Application.Interfaces;
using Pleiades.Web;
using Commerce.Application.Model.Orders;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        PushMarketContext Context { get; set; }
        IOrderSubmissionService OrderRepository { get; set; }

        public OrderController(PushMarketContext context, IOrderSubmissionService orderRepository)
        {
            this.Context = context;
            this.OrderRepository = orderRepository;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShippingMethods()
        {
            return new JsonNetResult(this.Context.ShippingMethods.ToList());
        }

        [HttpGet]
        public ActionResult States()
        {
            return new JsonNetResult(this.Context.StateTaxes.ToList());
        }

        [HttpPost]
        public ActionResult SubmitOrder(SubmitOrderRequest orderRequest)
        {
            var response = this.OrderRepository.Submit(orderRequest);
            return new JsonNetResult(response);
        }

        public ActionResult TestMethod()
        {
            var controller = new ProductRepository(Context, null, null);
            var x = new ProductController(null, null, null, null, null, null, null);
            return new ContentResult();
        }

        public ActionResult Manager()
        {
            return View();
        }
    }
}
