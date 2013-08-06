using System.Linq;
using System.Web.Mvc;
using Commerce.Persist.Database;
using Pleiades.Web;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Orders;

namespace Commerce.Web.Areas.Admin.Controllers
{
    // TODO: move this Controller to the WebAPI
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

        public ActionResult Manager()
        {
            return View();
        }
    }
}
