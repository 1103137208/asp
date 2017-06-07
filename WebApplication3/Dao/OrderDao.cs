using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using WebApplication3.Models;

namespace WebApplication3.Dao
{
    public class OrderDao
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
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@RequireDdate", order.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate == null ? string.Empty : order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID == System.Data.SqlTypes.SqlInt32.Null ? System.Data.SqlTypes.SqlInt32.Null : order.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight.ToString() == null ? 0 : order.Freight));
                cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName == null ? string.Empty : order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress == null ? string.Empty : order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity == null ? string.Empty : order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion == null ? string.Empty : order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode == null ? string.Empty : order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry == null ? string.Empty : order.ShipCountry));

                orderId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return orderId;

        }

        /// <summary>
		/// 新增訂單明細
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        public void InsertOrderDetail(List<OrderDetail> OrderDetail, int orderid)
        {

            string sql = @"INSERT INTO [Sales].[OrderDetails]
                            ([OrderID]
                            ,[ProductID]
                            ,[UnitPrice]
                            ,[Qty])
                        VALUES
                            (@OrderID
                            ,@ProductID
                            ,@UnitPrice
                            ,@Qty)";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                for (int i = 0; i < OrderDetail.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderID", orderid));
                    cmd.Parameters.Add(new SqlParameter("@ProductID", OrderDetail[i].ProductID));
                    cmd.Parameters.Add(new SqlParameter("@UnitPrice", OrderDetail[i].UnitPrice));
                    cmd.Parameters.Add(new SqlParameter("@Qty", OrderDetail[i].Qty));

                    cmd.ExecuteScalar();
                }

                conn.Close();
            }
        }

        /// <summary>
		/// 以ID取得訂單資料
		/// </summary>
		/// <param name="order"></param>
		/// <returns>訂單資料</returns>
        public DataTable GetOrderById(string orderId)
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

            return dt;


        }

        /// <summary>
        /// 以ID取得訂單資料(修改)
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public DataTable GetOrderByIdForUpdate(string orderId)
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

            return dt;
        }

        /// <summary>
		/// 查詢訂單
		/// </summary>
		/// <param name="order"></param>
		/// <returns>訂單資料</returns>
        public DataTable GetOrder()
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


            return dt;


        }
        
        /// <summary>
        /// 依照條件取得訂單資料
        /// </summary>
        /// <returns></returns>
        public DataTable GetOrderByCondtioin(Models.OrderSearchArg arg)
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


            return dt;
        }

        /// <summary>
		/// 依ID取得訂單明細
		/// </summary>
        /// 
        public DataTable GetOrderDetailById(string orderId)
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
            return dt;
        }

        /// <summary>
		/// 刪除訂單明細
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        public void DeleteOrderDetailById(string orderId)
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

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
		/// 刪除訂單
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
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

        /// <summary>
		/// 修改訂單
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        public void UpdateOrder(Models.Order order)
        {
            string sql = @"UPDATE [Sales].[Orders]
                            SET [CustomerID] = @CustomerID
                            ,[EmployeeID] =@EmployeeID
                            ,[OrderDate] = @Orderdate
                            ,[RequiredDate] = @RequireDdate
                            ,[ShippedDate] = @ShippedDate
                            ,[ShipperID] = @ShipperID
                            ,[Freight] =@Freight
                            ,[ShipName] = @ShipName
                            ,[ShipAddress] =@ShipAddress
                            ,[ShipCity] = @ShipCity
                            ,[ShipRegion] = @ShipRegion
                            ,[ShipPostalCode] =@ShipPostalCode
                            ,[ShipCountry] = @ShipCountry
                            WHERE OrderID=@OrderID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", order.CustomerID));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", order.Orderdate == null ? string.Empty : order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@RequireDdate", order.RequireDdate == null ? string.Empty : order.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate == null ? string.Empty : order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName == null ? string.Empty : order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress == null ? string.Empty : order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity == null ? string.Empty : order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion == null ? string.Empty : order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode == null ? string.Empty : order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry == null ? string.Empty : order.ShipCountry));
                cmd.Parameters.Add(new SqlParameter("@OrderID", order.OrderID));
                cmd.ExecuteScalar();
                conn.Close();
            }

        }

        /// <summary>
		/// 刪除訂單明細(修改)
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        public void DeleteOrderDetailForUpdate(string OrderID, string ProductID)
        {
            try
            {
                string sql = "DELETE FROM [Sales].[OrderDetails]  WHERE OrderID =@OrderID AND ProductID =@ProductID ";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                    cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
		/// 修改訂單明細
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        public void UpdateOrderDetail(OrderDetail OrderDetail, string OrderID)
        {
            string sql = "UPDATE [Sales].[OrderDetails] SET [UnitPrice] = @UnitPrice,[Qty] =@Qty  WHERE OrderID=@OrderID AND ProductID=@ProductID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                cmd.Parameters.Add(new SqlParameter("@ProductID", OrderDetail.ProductID));
                cmd.Parameters.Add(new SqlParameter("@UnitPrice", OrderDetail.UnitPrice));
                cmd.Parameters.Add(new SqlParameter("@Qty", OrderDetail.Qty));
                cmd.ExecuteScalar();
                conn.Close();
            }
        }


    }
}