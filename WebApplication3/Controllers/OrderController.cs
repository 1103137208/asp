using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class OrderController : Controller
    {
        Models.CodeService codeService = new Models.CodeService();
        //
        // GET: /Order/
        public ActionResult Index()
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp();
            ViewBag.ShipCodeData = this.codeService.GetShipper();
            Models.OrderService orderService = new Models.OrderService();
            ViewBag.Result = orderService.GetOrder();
            return View("Index");
        }

        public ActionResult InsertOrder()
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp();
            ViewBag.CustCodeData = this.codeService.GetCustomer();
            ViewBag.ShipCodeData = this.codeService.GetShipper();
            ViewBag.ProductCodeData = this.codeService.GetProduct();
            return View(new Models.Order());
            //return View("InsertOrder");
        }
        
        [HttpPost()]
        public ActionResult DoInsertOrder(Models.Order order)
        {
            Models.OrderService orderService = new Models.OrderService();
            int orderid=orderService.InsertOrder(order);
            orderService.InsertOrderDetail(order.OrderDetail, orderid);
            ModelState.Clear();
            return Index();
        }

        [HttpPost()]
        public ActionResult Index(Models.OrderSearchArg arg)
        {

            Models.OrderService orderService = new Models.OrderService();
            
            if (arg.DeleteOrderId != null)
            {
                int s = orderService.DeleteOrderDetailById(arg.DeleteOrderId);
                if (s == 1)
                {
                    ViewBag.Result = orderService.GetOrder();
                }
            }
            else
            {
                ViewBag.EmpCodeData = this.codeService.GetEmp();
                ViewBag.ShipCodeData = this.codeService.GetShipper();
                ViewBag.Result = orderService.GetOrderByCondtioin(arg);
            }

            return View("index");
        }

        [HttpPost()]
        public ActionResult DoUpdateOrder(Models.Order UpdateData)
        {
            Models.OrderService orderService = new Models.OrderService();
            orderService.UpdateOrder(UpdateData);
            ModelState.Clear();
            return Index();
        }

        [HttpGet]
        public ActionResult UpdateOrder(string Orderid)
        {
            Models.OrderService orderService = new Models.OrderService();
            Models.Order Order = orderService.GetOrderByIdForUpdate(Orderid);
            //ViewBag.OrderData = Order;
            ViewBag.Sum = orderService.SumOrderDetailPrice(Order.OrderDetail);
            ViewBag.EmpCodeData = new SelectList(codeService.GetEmp(), "Value", "Text", Order.EmployeeID);
            ViewBag.ShipCodeData = new SelectList(codeService.GetShipper(), "Value", "Text", Order.ShipperID);
            ViewBag.CustCodeData = new SelectList(codeService.GetCustomer(), "Value", "Text", Order.CustomerID);
            ViewBag.ProductCodeData = new SelectList(codeService.GetProduct(), "Value", "Text");
            

            return View(Order);
        }


        ///[HttpGet()]
        /// public JsonResult TestJson()
        /// {
        ///     var result = new Models.Order();
        ///     result.CustId = "GSS";
        ////   result.CustName = "瑞陽";
        ///   return this.Json(result, JsonRequestBehavior.AllowGet);
        /// }

    }
}