using Bogus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        // GET: 查詢頁面
        [HttpGet()]
        public ActionResult Index()
        {
            DataTable result = this.TestSql();
            ViewBag.result = result;
            if (OrderList.Count == 0)
            {
                SetShippersList();
                SetEmployeeList();
                SetCustomerList();
            }
            
            ViewBag.EmployeeName = EmployeeList;

            ViewBag.CompanyName = ShippersList;

            return View();
        }

        //POST: 查詢結果
        [HttpPost()]
        public ActionResult QueryResult(FormCollection form)
        {
            ViewBag.form = form;

            List<Models.InsertViewModel> ResultList = OrderList;

            for (int i = 1; i < form.Count; i++)
            {
                if (form[i] != "")
                {
                    var form_item = form.GetKey(i);
                    int src_index;
                    string src_value;

                    switch (form_item)
                    {
                        case "OrderID":
                            ResultList = ResultList.FindAll(item => item.OrderID == int.Parse(form[i]));
                            break;
                        case "ContactName":
                            ResultList = ResultList.FindAll(item => item.ContactName.Contains(form[i]));
                            break;
                        //case "EmployeeName":
                        //    src_index = int.Parse(form[i]);
                        //    src_value = (EmployeeList[src_index]).LastName;
                        //    ResultList = ResultList.FindAll(item => item.EmployeeName.Contains(src_value));
                        //    break;
                        //case "CompanyName":
                        //    src_index = int.Parse(form[i]);
                        //    src_value = ShippersList[src_index].CompanyName;
                        //    ResultList = ResultList.FindAll(item => item.CompanyName.Contains(src_value));
                        //    break;
                        case "OrderDate":
                            ResultList = ResultList.FindAll(item => item.OrderDate.ToString("yyyy-MM-dd") == form[i]);
                            break;
                        case "ShippedDate":
                            ResultList = ResultList.FindAll(item => item.ShippedDate.ToString("yyyy-MM-dd") == form[i]);
                            break;
                        case "RequiredDate":
                            ResultList = ResultList.FindAll(item => item.RequiredDate.ToString("yyyy-MM-dd") == form[i]);
                            break;
                    }
                }
            }

            ViewBag.List = ResultList;

            return View();
        }

        //GET: 修改頁面
        [HttpGet()]
        public ActionResult EditOrders(int id)
        {
            if (OrderList.Count == 0)
            {
                SetShippersList();
                SetEmployeeList();
                SetShippersList();
            }
            int list_index = OrderList.IndexOf(OrderList.Find(item => item.OrderID == id)); 

            //int CN_List_ID = -1;//CustomerList_id
            //int EN_List_ID = -1;//EmployeeList_id
            //int CpN_List_ID = -1;//ShippersList_id

            //for (int i = 0; i < 5; i++)
            //{
            //    if (CN_List_ID == -1)
            //    {
            //        if (CustomerList[i].Text == ((Models.InsertViewModel)OrderList[id]).ContactName)
            //        {
            //            CN_List_ID = i;
            //        }
            //    }

            //    if(EN_List_ID == -1)
            //    {
            //        if (EmployeeList[i].Text == ((Models.InsertViewModel)OrderList[id]).EmployeeName)
            //        {
            //            EN_List_ID = i;
            //        }
            //    }

            //    if (CpN_List_ID == -1)
            //    {
            //        if (ShippersList[i].Text == ((Models.InsertViewModel)OrderList[id]).CompanyName)
            //        {
            //            CpN_List_ID = i;
            //        }
            //    }
            //}


            //設定下拉式選單預設值
            var Edit_CustomerList = CustomerList;
            //Edit_CustomerList[CN_List_ID].Selected = true;

            var Edit_EmployeeList = EmployeeList;
            //Edit_EmployeeList[EN_List_ID].Selected = true;

            var Edit_ShippersList = ShippersList;
            //Edit_ShippersList[CpN_List_ID].Selected = true;

            ViewBag.ContactName = Edit_CustomerList;

            ViewBag.EmployeeName = Edit_EmployeeList;

            ViewBag.CompanyName = Edit_ShippersList;

            ViewBag.List = OrderList[list_index];
            return View();
        }

        //POST:修改訂單儲存
        [HttpPost()]
        public ActionResult EditOrders(Models.InsertViewModel UpdateData)
        {
            int Form_OrderID = UpdateData.OrderID;
            int form_ContactName = int.Parse(UpdateData.ContactName);
            int form_EmployeeName = int.Parse(UpdateData.EmployeeName);
            int form_CompanyName = int.Parse(UpdateData.CompanyName);

            int Update_Index = OrderList.IndexOf(OrderList.Find(item => item.OrderID == Form_OrderID));

            //OrderList[Update_Index] = new Models.InsertViewModel
            //{
            //    OrderID = Form_OrderID,
            //    ContactName = CustomerList[form_ContactName].ContactName,
            //    EmployeeName = EmployeeList[form_EmployeeName].LastName,
            //    OrderDate = Convert.ToDateTime(UpdateData.OrderDate),
            //    RequiredDate = Convert.ToDateTime(UpdateData.RequiredDate),
            //    ShippedDate = Convert.ToDateTime(UpdateData.ShippedDate),
            //    CompanyName = ShippersList[form_CompanyName].CompanyName,
            //    Freight = UpdateData.Freight,
            //    ShipCountry = UpdateData.ShipCountry,
            //    ShipCity = UpdateData.ShipCity,
            //    ShipRegion = UpdateData.ShipRegion,
            //    ShipPostalCode = UpdateData.ShipPostalCode,
            //    ShipAddress = UpdateData.ShipAddress
            //};

            ViewBag.EmployeeName = EmployeeList;

            ViewBag.CompanyName = ShippersList;
            return RedirectToAction("Index");
        }

        //新增訂單頁面
        [HttpGet()]
        public ActionResult InsertOrders()
        {
            if (OrderList.Count == 0)
            {
                SetShippersList();
                SetEmployeeList();
                SetShippersList();
            }


            ViewBag.ContactName = CustomerList;

            ViewBag.EmployeeName = EmployeeList;

            ViewBag.CompanyName = ShippersList;

            return View();
        }

        //POST: 新增訂單儲存
        [HttpPost()]
        public ActionResult InsertOrders(Models.InsertViewModel InsertData)
        {
            int form_ContactName = int.Parse(InsertData.ContactName);
            int form_EmployeeName = int.Parse(InsertData.EmployeeName);
            int form_CompanyName = int.Parse(InsertData.CompanyName);

            //OrderList.Add(new Models.InsertViewModel
            //{
            //    OrderID = OrderList[OrderList.Count-1].OrderID + 1,
            //    ContactName = CustomerList[form_ContactName].ContactName,
            //    EmployeeName = EmployeeList[form_EmployeeName].LastName,
            //    OrderDate = Convert.ToDateTime(InsertData.OrderDate),
            //    RequiredDate = Convert.ToDateTime(InsertData.RequiredDate),
            //    ShippedDate = Convert.ToDateTime(InsertData.ShippedDate),
            //    CompanyName = ShippersList[form_CompanyName].CompanyName,
            //    Freight = InsertData.Freight,
            //    ShipCountry = InsertData.ShipCountry,
            //    ShipCity = InsertData.ShipCity,
            //    ShipRegion = InsertData.ShipRegion,
            //    ShipPostalCode = InsertData.ShipPostalCode,
            //    ShipAddress = InsertData.ShipAddress
            //});

            ViewBag.EmployeeName = EmployeeList;

            ViewBag.CompanyName = ShippersList;
            //return View("Index");
            return RedirectToAction("Index");
        }

        //GET: 刪除訂單動作
        [HttpGet]
        public ActionResult DeleteOrders(int id)
        {
            OrderList.Remove(OrderList.Find(item => item.OrderID == id));

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

        public DataTable TestSql()
        {
            string connStr = this.GetConnStr();

            SqlConnection conn = new SqlConnection(connStr);

            string sql = "Select EmployeeID From [HR].[Employees]";

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conn);

            System.Data.DataSet data_result = new System.Data.DataSet();

            dataAdapter.Fill(data_result);
            
            return data_result.Tables[0];
        }
    }
}