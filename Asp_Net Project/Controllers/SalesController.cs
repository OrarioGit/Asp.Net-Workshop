using Bogus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asp_Net_Project.Controllers
{
    public class SalesController : Controller
    {
        static Bogus.Faker faker = new Faker("en");
        static List<Models.InsertViewModel> OrderList = new List<Models.InsertViewModel>();
        static List<SelectListItem> EmployeeList = new List<SelectListItem>();
        static List<SelectListItem> ShippersList = new List<SelectListItem>();
        static List<SelectListItem> CustomerList = new List<SelectListItem>();
        static List<SelectListItem> ProductsList = new List<SelectListItem>();

        // GET: 查詢頁面
        [HttpGet()]
        public ActionResult Index()
        {
            if (EmployeeList.Count == 0)
            {
                SetShippersList();
                SetEmployeeList();
                SetCustomerList();
                SetProductsList();
            }
            
            ViewBag.EmployeeList = EmployeeList;

            ViewBag.CompanyName = ShippersList;

            return View();
        }

        //POST: 查詢結果
        [HttpPost()]
        public ActionResult QueryResult(Models.QueryViewModel QueryData)
        {
            DataTable QueryResult = SearchOrdersInfo( QueryData.OrderID
                                                    , QueryData.ContactName
                                                    , QueryData.EmployeeName
                                                    , QueryData.CompanyName
                                                    , QueryData.OrderDate
                                                    , QueryData.ShippedDate
                                                    , QueryData.RequiredDate);

            //將ShippedDate中的null值替換
            for (int i = 0; i < QueryResult.Rows.Count; i++)
            {
                if (QueryResult.Rows[i]["ShippedDate"] == System.DBNull.Value)
                {
                    QueryResult.Rows[i]["ShippedDate"] = DateTime.MinValue;
                }
            }

            ViewBag.QueryResult = QueryResult;
            return View();
        }

        //GET: 修改頁面
        [HttpGet()]
        public ActionResult EditOrders(int id)
        {
            if (EmployeeList.Count == 0)
            {
                SetShippersList();
                SetEmployeeList();
                SetShippersList();
                SetProductsList();
            }

            //查詢訂單資料
            DataTable QueryResult = SearchOrderInfo(id);
            DataTable QueryDetalResult = SearchOrderDetailInfo(id);

            //int list_index = OrderList.IndexOf(OrderList.Find(item => item.OrderID == id)); 
            
            /*下次撰寫選單設定*/
            //設定下拉式選單預設值
            //var Edit_CustomerList = CustomerList;
            //Edit_CustomerList[CN_List_ID].Selected = true;

            //var Edit_EmployeeList = EmployeeList;
            //Edit_EmployeeList[EN_List_ID].Selected = true;

            //var Edit_ShippersList = ShippersList;
            //Edit_ShippersList[CpN_List_ID].Selected = true;

            ViewBag.ContactName = CustomerList;

            ViewBag.EmployeeName = EmployeeList;

            ViewBag.CompanyName = ShippersList;

            ViewBag.OrderInfo = QueryResult;
            ViewBag.OrderDetailInfo = QueryDetalResult;
            return View();
        }

        //POST:修改訂單儲存
        [HttpPost()]
        public ActionResult EditOrders(Models.InsertViewModel UpdateData)
        {
            UpdateOrder(UpdateData);
            
            ViewBag.EmployeeName = EmployeeList;

            ViewBag.CompanyName = ShippersList;
            return RedirectToAction("Index");
        }

        //GET: 新增訂單頁面
        [HttpGet()]
        public ActionResult InsertOrders()
        {
            if (EmployeeList.Count == 0)
            {
                SetShippersList();
                SetEmployeeList();
                SetShippersList();
                SetProductsList();
            }


            ViewBag.ContactName = CustomerList;

            ViewBag.EmployeeName = EmployeeList;

            ViewBag.CompanyName = ShippersList;

            ViewBag.ProductName = ProductsList;

            return View();
        }

        //POST: 新增訂單儲存
        [HttpPost()]
        public ActionResult InsertOrders(Models.InsertViewModel InsertData)
        {
            InsertOrder(InsertData);
            
            ViewBag.EmployeeName = EmployeeList;

            ViewBag.CompanyName = ShippersList;

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 獲取商品細節下拉式選單
        /// </summary>
        /// <param name="id"></param>
        /// <returns>商品細節下拉式選單</returns>
        [HttpGet()]
        public JsonResult GetProductsDetailList()
        {
            return Json(ProductsList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 獲取商品價格
        /// </summary>
        /// <param name="id"></param>
        /// <returns>商品價格</returns>
        [HttpGet()]
        public JsonResult GetProductPrice(int ProductID)
        {
            DataTable result = SearchProductPrice(ProductID);
            return Json(result.Rows[0]["UnitPrice"], JsonRequestBehavior.AllowGet);
        }

        //GET: 刪除訂單動作
        [HttpGet]
        public ActionResult DeleteOrders(int id)
        {
            DeleteOrder(id);

            ViewBag.EmployeeName = EmployeeList;

            ViewBag.CompanyName = ShippersList;

            return View("Index");
        }
        
        //建立負責員工下拉式選單data
        public void SetEmployeeList()
        {
            DataTable EmployeesInfo = SearchEmployeesInfo();
            for (int i = 0; i < EmployeesInfo.Rows.Count; i++)
            {
                string EmployeeName = EmployeesInfo.Rows[i]["FirstName"].ToString() + " " + EmployeesInfo.Rows[i]["LastName"].ToString();
                EmployeeList.Add(new SelectListItem()
                {
                    Text = EmployeeName,
                    Value = EmployeesInfo.Rows[i]["EmployeeID"].ToString(),
                    Selected = false
                });
            }
        }

        //建立運輸公司名稱下拉式選單data
        public void SetShippersList()
        {
            DataTable ShippersInfo = SearchShippersInfo();
            for (int j = 0; j < ShippersInfo.Rows.Count; j++)
            {
                ShippersList.Add(new SelectListItem()
                {
                    Text = ShippersInfo.Rows[j]["CompanyName"].ToString(),
                    Value = ShippersInfo.Rows[j]["ShipperID"].ToString(),
                    Selected = false
                });
            }
        }

        //建立客戶名稱下拉式選單data
        public void SetCustomerList()
        {
            DataTable CustomersInfo = SearchCustomersInfo();
            for (int j = 0; j < CustomersInfo.Rows.Count; j++)
            {
                CustomerList.Add(new SelectListItem()
                {
                    Text = CustomersInfo.Rows[j]["CompanyName"].ToString(),
                    Value = CustomersInfo.Rows[j]["CustomerID"].ToString(),
                    Selected = false
                });
            }
        }

        //建立產品名稱下拉式選單data
        public void SetProductsList()
        {
            DataTable ProductsInfo = SearchProductsInfo();
            for (int j = 0; j < ProductsInfo.Rows.Count; j++)
            {
                ProductsList.Add(new SelectListItem()
                {
                    Text = ProductsInfo.Rows[j]["ProductName"].ToString(),
                    Value = ProductsInfo.Rows[j]["ProductID"].ToString(),
                    Selected = false
                });
            }
        }

        /// <summary>
        /// 取得連線字串
        /// </summary>
        /// <returns>連線字串</returns>
        public string GetConnStr()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }

        /// <summary>
        /// 查詢客戶資料
        /// </summary>
        /// <returns>客戶資料</returns>
        public DataTable SearchCustomersInfo()
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Select CustomerID, CompanyName, ContactName " +
                         "From [Sales].[Customers]";

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conn);

            System.Data.DataSet data_result = new System.Data.DataSet();

            dataAdapter.Fill(data_result);

            return data_result.Tables[0];
        }

        /// <summary>
        /// 查詢員工資料
        /// </summary>
        /// <returns>員工資料</returns>
        public DataTable SearchEmployeesInfo()
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Select EmployeeID, FirstName, LastName " +
                         "From [HR].[Employees]";

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conn);

            System.Data.DataSet data_result = new System.Data.DataSet();

            dataAdapter.Fill(data_result);

            return data_result.Tables[0];
        }

        /// <summary>
        /// 查詢運輸公司資料
        /// </summary>
        /// <returns>運輸公司資料</returns>
        public DataTable SearchShippersInfo()
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Select ShipperID, CompanyName, Phone " +
                         "From [Sales].[Shippers]";

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conn);

            System.Data.DataSet data_result = new System.Data.DataSet();

            dataAdapter.Fill(data_result);

            return data_result.Tables[0];
        }

        /// <summary>
        /// 查詢產品資料
        /// </summary>
        /// <returns>產品資料</returns>
        public DataTable SearchProductsInfo()
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Select ProductID, ProductName " +
                         "From [Production].[Products]";

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conn);

            System.Data.DataSet data_result = new System.Data.DataSet();

            dataAdapter.Fill(data_result);

            return data_result.Tables[0];
        }

        /// <summary>
        /// 查詢產品價格
        /// </summary>
        /// <returns>產品價格</returns>
        public DataTable SearchProductPrice(int ProductID)
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Select UnitPrice " +
                         "From [Production].[Products] " +
                         "Where ProductID = @ProductID";

            //宣告SQLCommand物件
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            System.Data.DataSet data_result = new System.Data.DataSet();

            dataAdapter.Fill(data_result);

            return data_result.Tables[0];
        }


        /// <summary>
        /// 查詢訂單資料
        /// </summary>
        /// <returns>訂單資料</returns>
        public DataTable SearchOrdersInfo(int OrderID
                                        , string CustomerName
                                        , string EmployeeID
                                        , string ShipperID
                                        , DateTime OrderDate
                                        , DateTime ShippedDate
                                        , DateTime RequiredDate)
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Select * From " +
                         "( " +
                         "Select Orders.OrderID, Customers.CompanyName, Orders.OrderDate, Orders.ShippedDate, ROW_NUMBER() over(Order By Orders.OrderID) as ROWNUM " +
                         "From Sales.Orders " +
                         "Join Sales.Customers " +
                         "on Orders.CustomerID = Customers.CustomerID " +
                         "Where 1 = 1 ";
            

            //宣告SQLCommand物件
            SqlCommand cmd = new SqlCommand(sql, conn);

            //建立串接字串的物件
            StringBuilder StringBuilder = new StringBuilder();
            StringBuilder.Append(sql);

            DateTime CompareDate = Convert.ToDateTime("0001/1/1");

            if (OrderID != 0)
            {
                StringBuilder.Append(" And Orders.OrderID = @OrderID");
                cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
            }
            if (CustomerName != null)
            {
                StringBuilder.Append(" And Customers.CompanyName Like @CustomerName");
                cmd.Parameters.Add(new SqlParameter("@CustomerName", '%' + CustomerName + '%'));
            }
            if (EmployeeID != null)
            {
                StringBuilder.Append(" And Orders.EmployeeID = @EmployeeID");
                int List_Index = Convert.ToInt32(EmployeeID) - 1;

                var value = EmployeeList[List_Index].Value;
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", value));
            }
            if (ShipperID != null)
            {
                StringBuilder.Append(" And Orders.ShipperID = @ShipperID");
                int List_Index = Convert.ToInt32(ShipperID) - 1;

                var value = ShippersList[List_Index].Value;
                cmd.Parameters.Add(new SqlParameter("@ShipperID", value));
            }
            if (DateTime.Compare(OrderDate, CompareDate) > 0)
            {
                StringBuilder.Append(" And Orders.OrderDate = @OrderDate");
                cmd.Parameters.Add(new SqlParameter("@OrderDate", OrderDate));
            }
            if (DateTime.Compare(ShippedDate, CompareDate) > 0)
            {
                StringBuilder.Append(" And Orders.ShippedDate = @ShippedDate");
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", ShippedDate));
            }
            if (DateTime.Compare(RequiredDate, CompareDate) > 0)
            {
                StringBuilder.Append(" And Orders.RequiredDate = @RequiredDate");
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", RequiredDate));
            }

            //設定搜尋筆數
            StringBuilder.Append(" ) as Result_table Where ROWNUM >= 1" /*and ROWNUM <= 10"*/);
            sql = StringBuilder.ToString();

            //因為Sql有改動，所以重新設定一次
            cmd.CommandText = sql;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            System.Data.DataSet data_result = new System.Data.DataSet();

            dataAdapter.Fill(data_result);

            return data_result.Tables[0];
        }

        /// <summary>
        /// 查詢訂單資料
        /// </summary>
        /// <returns>訂單資料</returns>
        public DataTable SearchOrderInfo(int OrderID)
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Select Orders.OrderID" +
                              ", Orders.CustomerID" +
                              ", Orders.EmployeeID" +
                              ", Orders.OrderDate" +
                              ", Orders.RequiredDate" +
                              ", Orders.ShippedDate" +
                              ", Orders.ShipperID" +
                              ", Orders.Freight" +
                              ", Orders.ShipName" +
                              ", Orders.ShipAddress" +
                              ", Orders.ShipCity" +
                              ", Orders.ShipRegion" +
                              ", Orders.ShipPostalCode" +
                              ", Orders.ShipCountry " +
                         "From Sales.Orders " +
                         "Join Sales.Customers " +
                         "on Orders.CustomerID = Customers.CustomerID " +
                         "Where Orders.OrderID = @OrderID";


            //宣告SQLCommand物件
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            System.Data.DataSet data_result = new System.Data.DataSet();

            dataAdapter.Fill(data_result);

            return data_result.Tables[0];
        }

        /// <summary>
        /// 查詢訂單明細資料
        /// </summary>
        /// <returns>訂單資料</returns>
        public DataTable SearchOrderDetailInfo(int OrderID)
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Select ProductID" +
                              ", UnitPrice" +
                              ", Qty " +
                         "From Sales.OrderDetails " +
                         "Where OrderID = @OrderID";


            //宣告SQLCommand物件
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            System.Data.DataSet data_result = new System.Data.DataSet();

            dataAdapter.Fill(data_result);

            return data_result.Tables[0];
        }

        /// <summary>
        /// 新增訂單資料
        /// </summary>
        /// <returns></returns>
        public void InsertOrder(Models.InsertViewModel InsertData)
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);
            
            string sql = "Insert Into Sales.Orders (CustomerID" +
                                                 ", EmployeeID" +
                                                 ", OrderDate" +
                                                 ", RequiredDate" +
                                                 ", ShippedDate" +
                                                 ", ShipperID" +
                                                 ", Freight" +
                                                 ", ShipName" +
                                                 ", ShipAddress" +
                                                 ", ShipCity" +
                                                 ", ShipRegion" +
                                                 ", ShipPostalCode" +
                                                 ", ShipCountry) " +
                                           "Values (@CustomerID" +
                                                 ", @EmployeeID" +
                                                 ", @OrderDate" +
                                                 ", @RequiredDate" +
                                                 ", @ShippedDate" +
                                                 ", @ShipperID" +
                                                 ", @Freight" +
                                                 ", @ShipName" +
                                                 ", @ShipAddress" +
                                                 ", @ShipCity" +
                                                 ", @ShipRegion" +
                                                 ", @ShipPostalCode" +
                                                 ", @ShipCountry);" +
                                                 "Select @@IDENTITY As OrderID";

            //宣告SQLCommand物件
            SqlCommand cmd = new SqlCommand(sql, conn);

            int CustomerList_Index = int.Parse(InsertData.ContactName) -1;
            int EmployeeList_Index = int.Parse(InsertData.EmployeeName) -1;
            
            
            cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerList[CustomerList_Index].Value));
            cmd.Parameters.Add(new SqlParameter("@EmployeeID", EmployeeList[EmployeeList_Index].Value));
            cmd.Parameters.Add(new SqlParameter("@OrderDate", InsertData.OrderDate));
            cmd.Parameters.Add(new SqlParameter("@RequiredDate", InsertData.RequiredDate));

            if (InsertData.ShippedDate == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", SqlDateTime.MinValue));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", InsertData.ShippedDate));
            }

            if (InsertData.CompanyName == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipperID", ShippersList[0].Value));
            }
            else
            {
                int ShippersList_Index = int.Parse(InsertData.CompanyName) - 1;
                cmd.Parameters.Add(new SqlParameter("@ShipperID", ShippersList[ShippersList_Index].Value));
            }

            cmd.Parameters.Add(new SqlParameter("@Freight", InsertData.Freight));

            cmd.Parameters.Add(new SqlParameter("@ShipName", ""));

            if (InsertData.ShipAddress == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", ""));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", InsertData.ShipAddress));
            }

            if (InsertData.ShipCity == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipCity", ""));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShipCity", InsertData.ShipCity));
            }

            if (InsertData.ShipRegion == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", ""));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", InsertData.ShipRegion));
            }

            if (InsertData.ShipPostalCode == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", ""));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", InsertData.ShipPostalCode));
            }

            if (InsertData.ShipCountry == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", ""));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", InsertData.ShipCountry));
            }
            

            conn.Open();

            //從conn物件啟用Transaction
            SqlTransaction tran = conn.BeginTransaction();

            cmd.Transaction = tran;
            int new_OrderID;
            try
            {
                //cmd.ExecuteNonQuery();
                new_OrderID = Convert.ToInt32(cmd.ExecuteScalar());
                
                
                tran.Commit();

                
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }

            InsertOrderDetail(InsertData, new_OrderID);
        }

        /// <summary>
        /// 新增訂單明細資料
        /// </summary>
        /// <returns></returns>
        public void InsertOrderDetail(Models.InsertViewModel InsertData, int new_OrderID)
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Insert Into Sales.OrderDetails (OrderID" +
                                                       ", ProductID" +
                                                       ", UnitPrice)" +
                                                 "Values (@OrderID" +
                                                       ", @ProductID" +
                                                       ", @UnitPrice) ";

            //宣告SQLCommand物件
            SqlCommand cmd = new SqlCommand(sql, conn);

            for (int i = 0; i < InsertData.Details.Count; i++)
            {
                if (InsertData.Details[i].ProductID == 0)
                {
                    break;
                }
                cmd.Parameters.Add(new SqlParameter("@OrderID", new_OrderID));
                cmd.Parameters.Add(new SqlParameter("@ProductID", InsertData.Details[i].ProductID));
                cmd.Parameters.Add(new SqlParameter("@UnitPrice", InsertData.Details[i].UnitPrice));


                conn.Open();

                //從conn物件啟用Transaction
                SqlTransaction tran = conn.BeginTransaction();

                cmd.Transaction = tran;

                try
                {
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                }

            }


        }

        /// <summary>
        /// 更新訂單資料
        /// </summary>
        /// <returns></returns>
        public void UpdateOrder(Models.InsertViewModel UpdateData)
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Update Sales.Orders " +
                         "Set CustomerID = @CustomerID" +
                           ", EmployeeID = @EmployeeID" +
                           ", OrderDate = @OrderDate" +
                           ", RequiredDate = @RequiredDate" +
                           ", ShippedDate = @ShippedDate" +
                           ", ShipperID = @ShipperID" +
                           ", Freight = @Freight" +
                           ", ShipName = @ShipName" +
                           ", ShipAddress = @ShipAddress" +
                           ", ShipCity = @ShipCity" +
                           ", ShipRegion = @ShipRegion" +
                           ", ShipPostalCode = @ShipPostalCode" +
                           ", ShipCountry = @ShipCountry " +
                           "Where OrderID = @OrderID";

            //宣告SQLCommand物件
            SqlCommand cmd = new SqlCommand(sql, conn);

            int CustomerList_Index = int.Parse(UpdateData.ContactName) - 1;
            int EmployeeList_Index = int.Parse(UpdateData.EmployeeName) - 1;


            cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerList[CustomerList_Index].Value));
            cmd.Parameters.Add(new SqlParameter("@EmployeeID", EmployeeList[EmployeeList_Index].Value));
            cmd.Parameters.Add(new SqlParameter("@OrderDate", UpdateData.OrderDate));
            cmd.Parameters.Add(new SqlParameter("@RequiredDate", UpdateData.RequiredDate));

            if (UpdateData.ShippedDate == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", SqlDateTime.MinValue));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", UpdateData.ShippedDate));
            }

            if (UpdateData.CompanyName == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipperID", ShippersList[0].Value));
            }
            else
            {
                int ShippersList_Index = int.Parse(UpdateData.CompanyName) - 1;
                cmd.Parameters.Add(new SqlParameter("@ShipperID", ShippersList[ShippersList_Index].Value));
            }

            cmd.Parameters.Add(new SqlParameter("@Freight", UpdateData.Freight));

            cmd.Parameters.Add(new SqlParameter("@ShipName", ""));

            if (UpdateData.ShipAddress == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", ""));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", UpdateData.ShipAddress));
            }

            if (UpdateData.ShipCity == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipCity", ""));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShipCity", UpdateData.ShipCity));
            }

            if (UpdateData.ShipRegion == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", ""));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", UpdateData.ShipRegion));
            }

            if (UpdateData.ShipPostalCode == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", ""));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", UpdateData.ShipPostalCode));
            }

            if (UpdateData.ShipCountry == null)
            {
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", ""));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", UpdateData.ShipCountry));
            }

            cmd.Parameters.Add(new SqlParameter("@OrderID", UpdateData.OrderID));


            conn.Open();

            //從conn物件啟用Transaction
            SqlTransaction tran = conn.BeginTransaction();

            cmd.Transaction = tran;

            try
            {
                cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }

        }

        /// <summary>
        /// 刪除訂單資料
        /// </summary>
        /// <returns></returns>
        public void DeleteOrder(int OrderID)
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Delete From Sales.OrderDetails " +
                         "Where OrderDetails.OrderID = @orderID; " +
                         "Delete From Sales.Orders " +
                         "Where Orders.OrderID = @orderID";

            //宣告SQLCommand物件
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(new SqlParameter("@orderID", OrderID));
            
            conn.Open();

            //從conn物件啟用Transaction
            SqlTransaction tran = conn.BeginTransaction();

            cmd.Transaction = tran;

            try
            {
                cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }

        }
        
    }
}