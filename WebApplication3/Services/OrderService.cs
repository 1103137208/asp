using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using WebApplication3.Models;

namespace WebApplication3.Services
{
    public class OrderService
    {
        Dao.OrderDao orderDao= new Dao.OrderDao();

        /// <summary>
		/// 新增訂單
		/// </summary>
		/// <param name="order"></param>
		/// <returns>訂單編號</returns>
		public int InsertOrder(Models.Order order)
        {
            int orderId = orderDao.InsertOrder(order);
            return orderId;

        }

        /// <summary>
		/// 新增訂單明細
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        public void InsertOrderDetail(List<OrderDetail> OrderDetail,int orderid)
        {
            orderDao.InsertOrderDetail(OrderDetail, orderid);
        }

        /// <summary>
        /// 以ID查詢訂單
        /// </summary>
        /// <param name="order"></param>
        /// <returns>訂單內容</returns>
        public List<Models.Order> GetOrderById(string orderId)
        {
            DataTable dt = orderDao.GetOrderById(orderId);
            return this.MapOrderDataToList(dt);
            
        }

        /// <summary>
        /// 取得訂單內容(修改)
        /// </summary>
        /// <param name="order"></param>
        /// <returns>訂單內容</returns>
        public Order GetOrderByIdForUpdate(string orderId)
        {
            DataTable order = orderDao.GetOrderByIdForUpdate(orderId);
            List<Models.OrderDetail> OrderDetail =MapOrderDetailToList(orderDao.GetOrderDetailById(orderId));
            Models.Order result = MapOrder(order, OrderDetail);
            return result;
        }

        /// <summary>
        /// 訂單查詢
        /// </summary>
        /// <param name="order"></param>
        /// <returns>訂單</returns>
        public List<Models.Order> GetOrder()
        {
            DataTable dt = orderDao.GetOrder();
            return this.MapOrderDataToList(dt);
        }
        
        /// <summary>
        /// 依照條件取得訂單資料
        /// </summary>
        /// <returns></returns>
        public List<Models.Order> GetOrderByCondtioin(Models.OrderSearchArg arg)
        {
            DataTable dt =orderDao.GetOrderByCondtioin(arg);
            return this.MapOrderDataToList(dt);
        }

        /// <summary>
		/// 取得訂單明細
		/// </summary>
        /// 
        public List<Models.OrderDetail> GetOrderDetailById(string orderId)
        {
            DataTable dt = orderDao.GetOrderDetailById(orderId);
            return this.MapOrderDetailToList(dt);
        }

        /// <summary>
        /// 刪除訂單明細
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public void DeleteOrderDetailById(string orderId)
        {
            orderDao.DeleteOrderDetailById(orderId);
        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int DeleteOrderById(string orderId)
        {
            int result = orderDao.DeleteOrderById(orderId);
            return result;
        }

        /// <summary>
        /// 修改訂單
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public void UpdateOrder(Models.Order order)
        {
            orderDao.UpdateOrder(order);
        }
        

        /// <summary>
        /// 修改訂單明細
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public void UpdateOrderDetail(List<OrderDetail> OrderDetail, string OrderID)
        {
            
            List<OrderDetail> oldOrderDetail = MapOrderDetailToList(orderDao.GetOrderDetailById(OrderID));
            List<OrderDetail> NewOrderDetail = new List<Models.OrderDetail>();
            for(int i = 0; i < OrderDetail.Count; i++)
            {
                int c = 0;
                for(int j = 0; j < oldOrderDetail.Count; j++)
                {
                    if (OrderDetail[i].ProductID == oldOrderDetail[j].ProductID)
                    {
                        c = 1;
                        orderDao.UpdateOrderDetail(OrderDetail[i], OrderID);
                        oldOrderDetail[i].ProductID = null;
                    }
                }
                if (c == 0)
                {
                        NewOrderDetail.Add(OrderDetail[i]);
                }
            }
            if (NewOrderDetail.Count > 0)
            {
                orderDao.InsertOrderDetail(NewOrderDetail, Convert.ToInt32(OrderID));
            }
            for (int i = 0; i < oldOrderDetail.Count; i++)
            {
                if (oldOrderDetail[i].ProductID != null)
                {
                    orderDao.DeleteOrderDetailForUpdate(OrderID, oldOrderDetail[i].ProductID);
                }
            }
        }

        /// <summary>
        /// 資料格式整理為OrderList
        /// </summary>
        /// <param name="order"></param>
        /// <returns>訂單內容</returns>
        private List<Models.Order> MapOrderDataToList(DataTable orderData)
        {
            List<Models.Order> result = new List<Order>();


            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new Order()
                {
                    CustomerID = (int)row["CustomerID"],
                    CompanyName = row["CompanyName"].ToString(),
                    EmployeeID = (int)row["EmployeeID"],
                    EmployeeName = row["EmployeeName"].ToString(),
                    Freight = (decimal)row["Freight"],
                    Orderdate = row["Orderdate"].ToString(),
                    OrderID = row["OrderID"].ToString(),
                    RequireDdate = row["RequireDdate"].ToString(),
                    ShipAddress = row["ShipAddress"].ToString(),
                    ShipCity = row["ShipCity"].ToString(),
                    ShipCountry = row["ShipCountry"].ToString(),
                    ShipName = row["ShipName"].ToString(),
                    ShippedDate = row["ShippedDate"].ToString(),
                    ShipperID = (int)row["ShipperID"],
                    ShipperName = row["ShipperName"].ToString(),
                    ShipPostalCode = row["ShipPostalCode"].ToString(),
                    ShipRegion = row["ShipRegion"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 資料格式整理為OrderDetailList
        /// </summary>
        /// <param name="order"></param>
        /// <returns>訂單內容</returns>
        private List<Models.OrderDetail> MapOrderDetailToList(DataTable orderData)
        {
            List<Models.OrderDetail> result = new List<Models.OrderDetail>();
            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new OrderDetail()
                {
                    OrderID = row["OrderID"].ToString(),
                    ProductID = row["ProductID"].ToString(),
                    UnitPrice = Convert.ToDouble(row["UnitPrice"]),
                    Qty = Convert.ToInt32(row["Qty"])
                });
            }
            return result;
        }

        /// <summary>
        /// 資料格式整理為Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>訂單內容</returns>
        private Models.Order MapOrder(DataTable order, List<Models.OrderDetail> OrderDetail)
        {
            Models.Order result = new Models.Order();
            foreach (DataRow row in order.Rows)
            {
                result.CustomerID = (int)row["CustomerID"];
                result.CompanyName = row["CompanyName"].ToString();
                result.EmployeeID = (int)row["EmployeeID"];
                result.EmployeeName = row["EmployeeName"].ToString();
                result.Freight = (decimal)row["Freight"];
                result.Orderdate = ChangeDate(row["Orderdate"].ToString());
                result.OrderID = row["OrderID"].ToString();
                result.RequireDdate = ChangeDate(row["RequireDdate"].ToString());
                result.ShipAddress = row["ShipAddress"].ToString();
                result.ShipCity = row["ShipCity"].ToString();
                result.ShipCountry = row["ShipCountry"].ToString();
                result.ShipName = row["ShipName"].ToString();
                result.ShippedDate = ChangeDate(row["ShippedDate"].ToString());
                result.ShipperID = (int)row["ShipperID"];
                result.ShipperName = row["ShipperName"].ToString();
                result.ShipPostalCode = row["ShipPostalCode"].ToString();
                result.ShipRegion = row["ShipRegion"].ToString();
            }
            result.OrderDetail = OrderDetail;
            return result;
        }

        /// <summary>
        /// 轉換日期格式
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private string ChangeDate(string date)
        {
            DateTime datetime = Convert.ToDateTime(date);
            string d = datetime.ToString("yyyy-MM-dd");


            return d;
        }

    }

  
}
