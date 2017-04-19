using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class OrderSearchArg
    {
        public string CompanyName { get; set; }
        public string OrderDate { get; set; }
        public string EmployeeID { get; set; }
        public string ShipperID { get; set; }
        public string DeleteOrderId { get; set; }
        public string RequireDdate { get; set; }
        public string ShippedDate { get;set;}
        public string OrderID { get; set; }
        public string UpdateOrderId { get; set; }
        public string ProductID { get; set; }
    }
}