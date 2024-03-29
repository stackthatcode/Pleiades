﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pleiades.Web;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Orders;
using Commerce.Persist.Model.Billing;
using Commerce.Persist.Model.Products;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    // TODO: move this Controller to the WebAPI
    public class OrderController : Controller
    {
        PleiadesContext Context { get; set; }
        IOrderSubmissionService OrderRepository { get; set; }

        public OrderController(PleiadesContext context, IOrderSubmissionService orderRepository)
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
