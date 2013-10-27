using System;
using System.Web.Mvc;
using Commerce.Application.Interfaces;
using Pleiades.Web.Json;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class ManageOrderController : Controller
    {
        private readonly IOrderManager _orderManager;

        public ManageOrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public ViewResult Editor()
        {
            return View();            
        }

        public JsonNetResult Search(int? orderStatus, string startDate, string endDate)
        {
            var complete = orderStatus.HasValue ? (orderStatus.Value == 1) : (bool?)null;
            var result = _orderManager.Find(DateTime.Parse(startDate), DateTime.Parse(endDate), complete);
            return new JsonNetResult(result);
        }

        public JsonNetResult Get(string externalId)
        {
            var result = _orderManager.Retrieve(externalId);
            return new JsonNetResult(result);
        }
    }
}
