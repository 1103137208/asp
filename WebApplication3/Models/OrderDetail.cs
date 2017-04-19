using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class OrderDetail
    {
        public string OrderID { get; set; }

        public string ProductID { get; set; }

        public int UnitPrice { get; set; }

        public int Qty { get; set; }
    }
}