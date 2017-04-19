using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Order
    {

        /// <summary>
        /// 建構式
        /// </summary>
        public Order()
        {
            var ods = new List<Models.OrderDetail>();
            ods.Add(new OrderDetail() { ProductID = 58.ToString() });
            this.OrderDetail = ods;

        }
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 客戶代號
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 業務(員工)代號
        /// </summary>
        public int EmployeeID { get; set; }

        /// <summary>
        /// 業務(員工姓名)
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 訂單日期
        /// </summary>
        public string Orderdate { get; set; }

        /// <summary>
        /// 需要日期
        /// </summary>
        public string RequireDdate { get; set; }

        /// <summary>
        /// 出貨日期
        /// </summary>
        public string ShippedDate { get; set; }

        /// <summary>
        /// 出貨公司代號
        /// </summary>
        public int ShipperID { get; set; }

        /// <summary>
        /// 出貨公司名稱
        /// </summary>
        public string ShipperName { get; set; }

        /// <summary>
        /// 運費
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 出貨說明
        /// </summary>
        public string ShipName { get; set; }

        /// <summary>
        /// 出貨地址
        /// </summary>
        public string ShipAddress { get; set; }

        /// <summary>
        /// 出貨程式
        /// </summary>
        public string ShipCity { get; set; }

        /// <summary>
        /// 出貨地區
        /// </summary>
        public string ShipRegion { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string ShipPostalCode { get; set; }

        /// <summary>
        /// 出貨國家
        /// </summary>
        public string ShipCountry { get; set; }

        /// <summary>
        /// 訂單明細
        /// </summary>
        public List<OrderDetail> OrderDetail { get; set; }
    }
}