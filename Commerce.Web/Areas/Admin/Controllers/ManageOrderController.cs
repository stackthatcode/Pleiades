using System;
using System.Linq;
using System.Web.Mvc;
using Commerce.Application.Database;
using Commerce.Application.Orders;
using Pleiades.Web.Json;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class ManageOrderController : Controller
    {
        private readonly IOrderManager _orderManager;
        private PushMarketContext _context;

        public ManageOrderController(IOrderManager orderManager, PushMarketContext context)
        {
            _orderManager = orderManager;
            _context = context;
        }

        public ViewResult Editor()
        {
            return View();            
        }

        public JsonNetResult Search(int? orderStatus, string startDate, string endDate)
        {
            var complete = orderStatus.HasValue ? (orderStatus.Value == 2) : (bool?)null;
            var result = _orderManager.Find(DateTime.Parse(startDate), DateTime.Parse(endDate), complete);
            return new JsonNetResult(result);
        }

        public JsonNetResult Get(string externalId)
        {
            var result = _orderManager.Retrieve(externalId);
            return new JsonNetResult(result);
        }

        [HttpPost]
        public JsonNetResult Ship(string externalId, string orderLineIds)
        {
            var orderLines = orderLineIds.Split(',').Select(Int32.Parse).ToList();
            var shipment = _orderManager.Ship(externalId, orderLines);
            _context.SaveChanges();
            return new JsonNetResult(shipment.Order);
        }

        [HttpPost]
        public JsonNetResult Refund(string externalId, string orderLineIds)
        {
            var orderLines = orderLineIds.Split(',').Select(Int32.Parse).ToList();
            var refund = _orderManager.Refund(externalId, orderLines);
            _context.SaveChanges();
            //var order = _orderManager.Retrieve(externalId);
            return new JsonNetResult(refund.Order);
        }
    }
}
