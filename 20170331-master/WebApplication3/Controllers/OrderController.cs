using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/
        public ActionResult Index()
        {
            List<SelectListItem> custData = new List<SelectListItem>();
            custData.Add(new SelectListItem()
            {
                Text = "瑞陽資訊",
                Value = "1"
            });
            custData.Add(new SelectListItem()
            {
                Text = "往阮資訊",
                Value = "2"
            });

            ViewBag.custData = custData;
            return View();
        }
        public ActionResult InsertOrder()
        {
            return View();
        }
        [HttpPost()]
        public ActionResult InsertOrder(Models.Order order)
        {
            Models.OrderService orderService = new Models.OrderService();
            orderService.InsertOrder(order);
            return View("Index");
        }

        [HttpGet()]
        public JsonResult TestJson()
        {
            var result = new Models.Order();
            result.CustId = "GSS";
            result.CustName = "瑞陽";

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}