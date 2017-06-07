using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace WebApplication3.Controllers
{
    public class OrderController : Controller
    {
        Services.CodeService codeService = new Services.CodeService();
        //
        // GET: /Order/

        /// <summary>
        /// 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
		/// 新增訂單頁面
		/// </summary>
        public ActionResult InsertOrder()
        {
            return View(new Models.Order());
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        [HttpPost()]
        public JsonResult DoInsertOrder(Models.Order order)
        {
            Services.OrderService orderService = new Services.OrderService();
            int orderid = orderService.InsertOrder(order);
            orderService.InsertOrderDetail(order.OrderDetail, orderid);
            ModelState.Clear();
            JsonResult result = this.Json(orderid, JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// 首頁資料回傳
        /// </summary>
        [HttpPost()]
        public JsonResult SelectOrder(Models.OrderSearchArg arg)
        {
            try
            {
                Services.OrderService orderService = new Services.OrderService();
                JsonResult result = this.Json(orderService.GetOrderByCondtioin(arg), JsonRequestBehavior.AllowGet);
                return result;

            }
            catch (Exception)
            {

                return this.Json(false);
            }
        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        [HttpPost()]
        public JsonResult DeleteOrder(Models.DeleteJson order)
        {
            try
            {
                string orderid = order.OrderID;
                Services.OrderService orderService = new Services.OrderService();
                orderService.DeleteOrderDetailById(orderid);
                int result = orderService.DeleteOrderById(orderid);
                if (result >= 1)
                {
                    return this.Json(true);
                }else
                {
                    return this.Json(false);
                }


            }
            catch (Exception)
            {

                return this.Json(false);
            }

        }

        /// <summary>
		/// 修改訂單
		/// </summary>
        [HttpPost()]
        public ActionResult DoUpdateOrder(Models.Order UpdateData)
        {

            Services.OrderService orderService = new Services.OrderService();
            orderService.UpdateOrder(UpdateData);
            orderService.UpdateOrderDetail(UpdateData.OrderDetail, UpdateData.OrderID);
            ModelState.Clear();
            return Index();
        }

        /// <summary>
		/// 修改訂單頁面
		/// </summary>
        [HttpGet]
        public ActionResult UpdateOrder(string Orderid)
        {
            Services.OrderService orderService = new Services.OrderService();
            Models.Order Order = orderService.GetOrderByIdForUpdate(Orderid);
            ViewBag.OrderData = Order;
            return View();
        }

        /// <summary>
		/// 刪除訂單明細
		///// </summary>
  //      [HttpPost]
  //      public JsonResult DeleteOrderDetail(Models.DeleteJson orderdetail)
  //      {
  //          try
  //          {
  //              string orderid = orderdetail.OrderID;
  //              string productid = orderdetail.ProductID;
  //              Services.OrderService orderService = new Services.OrderService();
  //              int result = orderService.DeleteOrderDetailForUpdate(orderid, productid);
  //              if (result == 1)
  //              {
  //                  return this.Json(true);
  //              }
  //              else
  //              {
  //                  return this.Json(false);
  //              }

  //          }
  //          catch (Exception)
  //          {

  //              return this.Json(false);
  //          }

  //      }

        /// <summary>
		/// Read
		/// </summary>
        public JsonResult Read(Models.OrderSearchArg arg)
        {
            try
            {
                Services.OrderService orderService = new Services.OrderService();
                JsonResult result = this.Json(orderService.GetOrderByCondtioin(arg), JsonRequestBehavior.AllowGet);
                return result;

            }
            catch (Exception)
            {

                return this.Json(false);
            }

        }

        /// <summary>
		/// 取得員工資料
		/// </summary>
        public JsonResult GetEmployeeList()
        {
            try
            {
                Services.CodeService codeService = new Services.CodeService();
                JsonResult result = this.Json(codeService.GetEmp(), JsonRequestBehavior.AllowGet);
                return result;

            }
            catch (Exception)
            {

                return this.Json(false);
            }
        }

        /// <summary>
		/// 取得送貨公司
		/// </summary>
        public JsonResult GetShipperList()
        {
            try
            {
                Services.CodeService codeService = new Services.CodeService();
                JsonResult result = this.Json(codeService.GetShipper(), JsonRequestBehavior.AllowGet);
                return result;

            }
            catch (Exception)
            {

                return this.Json(false);
            }
        }
        
        /// <summary>
        /// 取得客戶資料
        /// </summary>
        public JsonResult GetCustomerList()
        {
            try
            {
                Services.CodeService codeService = new Services.CodeService();
                JsonResult result = this.Json(codeService.GetCustomer(), JsonRequestBehavior.AllowGet);
                return result;

            }
            catch (Exception)
            {

                return this.Json(false);
            }
        }

        /// <summary>
        /// 取得客戶資料
        /// </summary>
        public JsonResult GetProductList()
        {
            try
            {
                Services.CodeService codeService = new Services.CodeService();
                JsonResult result = this.Json(codeService.GetProduct(), JsonRequestBehavior.AllowGet);
                return result;

            }
            catch (Exception)
            {

                return this.Json(false);
            }
        }

        /// <summary>
        /// 取得客戶資料
        /// </summary>
        public JsonResult GetProductPrice()
        {
            try
            {
                Services.ProductService productService = new Services.ProductService();
                JsonResult result = this.Json(productService.GetProduct(), JsonRequestBehavior.AllowGet);
                return result;

            }
            catch (Exception)
            {

                return this.Json(false);
            }
        }
    }
}