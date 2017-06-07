using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication3.Models
{
    public class OrderService
    {

        /// <summary>
		/// 取得DB連線字串
		/// </summary>
		/// <returns></returns>
		private string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString.ToString();
        }

        /// <summary>
		/// 新增訂單
		/// </summary>
		/// <param name="order"></param>
		/// <returns>訂單編號</returns>
		public int InsertOrder(Models.Order order)
        {
            string sql = @" Insert INTO Sales.Orders
						 (
							CustomerID,EmployeeID,Orderdate,RequireDdate,ShippedDate,ShipperID,Freight,
							ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry
						)
						VALUES
						(
							@CustomerID,@EmployeeID,@OrderDate,@RequireDdate,@ShippedDate,@ShipperID,@Freight,
							@ShipName,@ShipAddress,@ShipCity,@ShipRegion,@ShipPostalCode,@ShipCountry
						)
						Select SCOPE_IDENTITY()
						";
            int orderId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", order.CustomerID));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID",order.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@RequireDdate", order.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));

                orderId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return orderId;

        }
        public List<Models.Order> GetOrderById(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT 
					A.OrderId,A.CustomerID,B.Companyname As CompanyName,
					A.EmployeeID,C.Lastname+ C.Firstname As EmployeeName,
					A.Orderdate,A.RequireDdate,A.ShippedDate,
					A.ShipperId,D.companyname As ShipperName,A.Freight,
					A.ShipName,A.ShipAddress,A.ShipCity,A.ShipRegion,A.ShipPostalCode,A.ShipCountry
					From Sales.Orders As A 
					INNER JOIN Sales.Customers As B ON A.CustomerID=B.CustomerID
					INNER JOIN HR.Employees As C On A.EmployeeID=C.EmployeeID
					inner JOIN Sales.Shippers As D ON A.ShipperID=D.ShipperID
					Where  A.OrderID=@OrderID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderID", orderId));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            return this.MapOrderDataToList(dt);


        }

        public Order GetOrderByIdForUpdate(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT 
					A.OrderId,A.CustomerID,B.Companyname As CompanyName,
					A.EmployeeID,C.Lastname+ C.Firstname As EmployeeName,
					A.Orderdate,A.RequireDdate,A.ShippedDate,
					A.ShipperId,D.companyname As ShipperName,A.Freight,
					A.ShipName,A.ShipAddress,A.ShipCity,A.ShipRegion,A.ShipPostalCode,A.ShipCountry
					From Sales.Orders As A 
					INNER JOIN Sales.Customers As B ON A.CustomerID=B.CustomerID
					INNER JOIN HR.Employees As C On A.EmployeeID=C.EmployeeID
					inner JOIN Sales.Shippers As D ON A.ShipperID=D.ShipperID
					Where  A.OrderID=@OrderID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderID", orderId));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            Models.Order result = new Models.Order();
            foreach(DataRow row in dt.Rows)
            {
                result.CustomerID = (int)row["CustomerID"];
                result.CompanyName = row["CompanyName"].ToString();
                result.EmployeeID = (int)row["EmployeeID"];
                result.EmployeeName = row["EmployeeName"].ToString();
                result.Freight = (decimal)row["Freight"];
                result.Orderdate = row["Orderdate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["Orderdate"];
                result.OrderID = row["OrderID"].ToString();
                result.RequireDdate = row["RequireDdate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["RequireDdate"];
                result.ShipAddress = row["ShipAddress"].ToString();
                result.ShipCity = row["ShipCity"].ToString();
                result.ShipCountry = row["ShipCountry"].ToString();
                result.ShipName = row["ShipName"].ToString();
                result.ShippedDate = row["ShippedDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["ShippedDate"];
                result.ShipperID = (int)row["ShipperID"];
                result.ShipperName = row["ShipperName"].ToString();
                result.ShipPostalCode = row["ShipPostalCode"].ToString();
                result.ShipRegion = row["ShipRegion"].ToString();
            } 
            return result;
        }

        public List<Models.Order> GetOrder()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT 
					A.OrderId,A.CustomerID,B.Companyname As CompanyName,
					A.EmployeeID,C.Lastname+ C.Firstname As EmployeeName,
					A.Orderdate,A.RequireDdate,A.ShippedDate,
					A.ShipperId,D.companyname As ShipperName,A.Freight,
					A.ShipName,A.ShipAddress,A.ShipCity,A.ShipRegion,A.ShipPostalCode,A.ShipCountry
					From Sales.Orders As A 
					INNER JOIN Sales.Customers As B ON A.CustomerID=B.CustomerID
					INNER JOIN HR.Employees As C On A.EmployeeID=C.EmployeeID
					inner JOIN Sales.Shippers As D ON A.ShipperID=D.ShipperID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }


            return this.MapOrderDataToList(dt);


        }


        /// <summary>
        /// 依照條件取得訂單資料
        /// </summary>
        /// <returns></returns>
        public List<Models.Order> GetOrderByCondtioin(Models.OrderSearchArg arg)
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT 
					A.OrderId,A.CustomerID,B.Companyname As CompanyName,
					A.EmployeeID,C.Lastname+ C.Firstname As EmployeeName,
					A.Orderdate,A.RequireDdate,A.ShippedDate,
					A.ShipperId,D.companyname As ShipperName,A.Freight,
					A.ShipName,A.ShipAddress,A.ShipCity,A.ShipRegion,A.ShipPostalCode,A.ShipCountry
					From Sales.Orders As A 
					INNER JOIN Sales.Customers As B ON A.CustomerID=B.CustomerID
					INNER JOIN HR.Employees As C On A.EmployeeID=C.EmployeeID
					inner JOIN Sales.Shippers As D ON A.ShipperID=D.ShipperID
					Where (B.Companyname Like @CompanyName Or @CompanyName='') And
                          (A.OrderId=@OrderID Or @OrderID='') And
                          (A.EmployeeID= @EmployeeID Or @EmployeeID='') And
                          (A.ShipperId = @ShipperID Or @ShipperID='') And
                          (A.Orderdate = @Orderdate Or @Orderdate='') And
                          (A.ShippedDate= @ShippedDate Or @ShippedDate='') And
                          (A.RequireDdate = @RequireDdate Or @RequireDdate='')";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CompanyName", arg.CompanyName == null ? string.Empty : '%' + arg.CompanyName + '%'));
                cmd.Parameters.Add(new SqlParameter("@OrderID", arg.OrderID == null ? string.Empty : arg.OrderID));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", arg.EmployeeID == null ? string.Empty : arg.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", arg.ShipperID == null ? string.Empty : arg.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", arg.ShippedDate == null ? string.Empty : arg.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@RequireDdate", arg.RequireDdate == null ? string.Empty : arg.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", arg.OrderDate == null ? string.Empty : arg.OrderDate));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }


            return this.MapOrderDataToList(dt);
        }

        /// <summary>
		/// 刪除訂單
		/// </summary>
        /// 
        public List<Models.OrderDetail> GetOrderDetailById(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT OrderID,ProductID,UnitPrice,Qty FROM Sales.OrderDetails Where OrderID  = @orderid";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@orderid", orderId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDetailToList(dt);
        }


        public int DeleteOrderDetailById(string orderId)
        {
            try
            {
                int result;
                string sql = "Delete FROM Sales.OrderDetails Where OrderID=@orderid";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@orderid", orderId));
                    result = cmd.ExecuteNonQuery();
                    conn.Close();
                }
                if (result == 1)
                {
                    result = DeleteOrderById(orderId);
                    return result;
                }
                else
                {
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public int DeleteOrderById(string orderId)
        {
            try
            {
                int result;
                string sql = "Delete FROM Sales.Orders Where OrderID=@orderid";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@orderid", orderId));
                    result = cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateOrder(Models.Order order)
        {
        }

        ///public List<Models.Order> GetOrders()
        ///{
        ///List<Models.Order> result = new List<Order>();
        ///result.Add(new Order() { CustId = "GSS", CustName = "瑞陽資訊", EmpId = 1, EmpName = "逆好", Orderdate = DateTime.Parse("2015/11/08") });
        ///result.Add(new Order() { CustId = "NPOIS", CustName = "往阮資訊", EmpId = 2, EmpName = "逆好2", Orderdate = DateTime.Parse("2015/11/01") });
        ///return result;
        ///}
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
                    Orderdate = row["Orderdate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["Orderdate"],
                    OrderID = row["OrderID"].ToString(),
                    RequireDdate = row["RequireDdate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["RequireDdate"],
                    ShipAddress = row["ShipAddress"].ToString(),
                    ShipCity = row["ShipCity"].ToString(),
                    ShipCountry = row["ShipCountry"].ToString(),
                    ShipName = row["ShipName"].ToString(),
                    ShippedDate = row["ShippedDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["ShippedDate"],
                    ShipperID = (int)row["ShipperID"],
                    ShipperName = row["ShipperName"].ToString(),
                    ShipPostalCode = row["ShipPostalCode"].ToString(),
                    ShipRegion = row["ShipRegion"].ToString()
                });
            }
            return result;
        }

        private List<Models.OrderDetail> MapOrderDetailToList(DataTable orderData)
        {
            List<Models.OrderDetail> result = new List<OrderDetail>();
            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new OrderDetail()
                {
                    OrderID = row["OrderID"].ToString(),
                    ProductID = row["ProductID"].ToString(),
                    UnitPrice = Convert.ToInt32(row["UnitPrice"]),
                    Qty = Convert.ToInt32(row["Qty"])
                });
            }
            return result;
        }

        public int SumOrderDetailPrice(List<Models.OrderDetail> OrderDetail)
        {
            int sum = 0;
            foreach(var row in OrderDetail)
            {
                sum += row.UnitPrice * row.Qty;
            }
            return sum;
        }
    }

  
}
