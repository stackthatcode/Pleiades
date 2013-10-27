using System.Linq;
using System.Web.Mvc;
using Pleiades.Web.Json;
using Commerce.Application.Database;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Orders;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class CreateOrderController : Controller
    {
        PushMarketContext Context { get; set; }
        IOrderService OrderRepository { get; set; }

        public CreateOrderController(PushMarketContext context, IOrderService orderRepository)
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
        public ActionResult SubmitOrder(OrderRequest orderRequest)
        {
            var response = this.OrderRepository.Submit(orderRequest);
            return new JsonNetResult(response);
        }

        public ActionResult Manage()
        {
            return View();
        }
    }
}
