using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asp_Net_Project.Controllers
{
    public class SalesController : Controller
    {
        static Bogus.Faker faker = new Faker("en");
        static List<Models.InsertViewModel> OrderList = new List<Models.InsertViewModel>();
        static List<Models.Employees> EmployeeList = new List<Models.Employees>();
        static List<Models.Customers> CustomerList = new List<Models.Customers>();
        static List<Models.Shippers> ShippersList = new List<Models.Shippers>();
        static List<SelectListItem> EmployeeNameList = new List<SelectListItem>();
        static List<SelectListItem> CompanyNameList = new List<SelectListItem>();
        static List<SelectListItem> ContactNameList = new List<SelectListItem>();

        // GET: 查詢頁面
        [HttpGet()]
        public ActionResult Index()
        {
            if (OrderList.Count == 0)
            {
                DataSetUp();
                SetCompanyNameList();
                SetEmployeeNameList();
                SetContactNameList();
            }


            Models.QueryViewModel QueryModel = new Models.QueryViewModel();


            ViewBag.EmployeeName = EmployeeNameList;

            ViewBag.CompanyName = CompanyNameList;

            return View(QueryModel);
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
                        case "EmployeeName":
                            src_index = int.Parse(form[i]);
                            src_value = (EmployeeList[src_index]).LastName;
                            ResultList = ResultList.FindAll(item => item.EmployeeName.Contains(src_value));
                            break;
                        case "CompanyName":
                            src_index = int.Parse(form[i]);
                            src_value = ShippersList[src_index].CompanyName;
                            ResultList = ResultList.FindAll(item => item.CompanyName.Contains(src_value));
                            break;
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
                DataSetUp();
                SetCompanyNameList();
                SetEmployeeNameList();
                SetCompanyNameList();
            }
            int list_index = OrderList.IndexOf(OrderList.Find(item => ((Models.InsertViewModel)item).OrderID == id)); 

            //int CN_List_ID = -1;//ContactNameList_id
            //int EN_List_ID = -1;//EmployeeNameList_id
            //int CpN_List_ID = -1;//CompanyNameList_id

            //for (int i = 0; i < 5; i++)
            //{
            //    if (CN_List_ID == -1)
            //    {
            //        if (ContactNameList[i].Text == ((Models.InsertViewModel)OrderList[id]).ContactName)
            //        {
            //            CN_List_ID = i;
            //        }
            //    }

            //    if(EN_List_ID == -1)
            //    {
            //        if (EmployeeNameList[i].Text == ((Models.InsertViewModel)OrderList[id]).EmployeeName)
            //        {
            //            EN_List_ID = i;
            //        }
            //    }

            //    if (CpN_List_ID == -1)
            //    {
            //        if (CompanyNameList[i].Text == ((Models.InsertViewModel)OrderList[id]).CompanyName)
            //        {
            //            CpN_List_ID = i;
            //        }
            //    }
            //}


            //設定下拉式選單預設值
            var Edit_ContactNameList = ContactNameList;
            //Edit_ContactNameList[CN_List_ID].Selected = true;

            var Edit_EmployeeNameList = EmployeeNameList;
            //Edit_EmployeeNameList[EN_List_ID].Selected = true;

            var Edit_CompanyNameList = CompanyNameList;
            //Edit_CompanyNameList[CpN_List_ID].Selected = true;

            ViewBag.ContactName = Edit_ContactNameList;

            ViewBag.EmployeeName = Edit_EmployeeNameList;

            ViewBag.CompanyName = Edit_CompanyNameList;

            ViewBag.List = OrderList[list_index];
            return View();
        }

        //POST:修改訂單儲存
        [HttpPost()]
        public ActionResult EditOrders(FormCollection form)
        {
            int Form_OrderID = int.Parse(form.Get("OrderID"));
            int form_ContactName = int.Parse(form.Get("ContactName"));
            int form_EmployeeName = int.Parse(form.Get("EmployeeName"));
            int form_CompanyName = int.Parse(form.Get("CompanyName"));

            int Update_Index = OrderList.IndexOf(OrderList.Find(item => item.OrderID == Form_OrderID));

            OrderList[Update_Index] = new Models.InsertViewModel
            {
                OrderID = Form_OrderID,
                ContactName = CustomerList[form_ContactName].ContactName,
                EmployeeName = EmployeeList[form_EmployeeName].LastName,
                OrderDate = Convert.ToDateTime(form.Get("OrderDate")),
                RequiredDate = Convert.ToDateTime(form.Get("RequiredDate")),
                ShippedDate = Convert.ToDateTime(form.Get("ShippedDate")),
                CompanyName = ShippersList[form_CompanyName].CompanyName,
                Freight = int.Parse(form.Get("Freight")),
                ShipCountry = form.Get("ShipCountry"),
                ShipCity = form.Get("ShipCity"),
                ShipRegion = form.Get("ShipRegion"),
                ShipPostalCode = form.Get("ShipPostalCode"),
                ShipAddress = form.Get("ShipAddress")
            };

            ViewBag.EmployeeName = EmployeeNameList;

            ViewBag.CompanyName = CompanyNameList;
            return View("Index");
        }

        //新增訂單頁面
        [HttpGet()]
        public ActionResult InsertOrders()
        {
            if (OrderList.Count == 0)
            {
                DataSetUp();
                SetCompanyNameList();
                SetEmployeeNameList();
                SetCompanyNameList();
            }


            ViewBag.ContactName = ContactNameList;

            ViewBag.EmployeeName = EmployeeNameList;

            ViewBag.CompanyName = CompanyNameList;

            return View();
        }

        //POST: 新增訂單儲存
        [HttpPost()]
        public ActionResult InsertOrders(/*FormCollection form*/Models.InsertViewModel InsertData)
        {
            int form_ContactName = int.Parse(InsertData.ContactName);
            int form_EmployeeName = int.Parse(InsertData.EmployeeName);
            int form_CompanyName = int.Parse(InsertData.CompanyName);

            OrderList.Add(new Models.InsertViewModel
            {
                OrderID = OrderList[OrderList.Count-1].OrderID + 1,
                ContactName = CustomerList[form_ContactName].ContactName,
                EmployeeName = EmployeeList[form_EmployeeName].LastName,
                OrderDate = Convert.ToDateTime(InsertData.OrderDate),
                RequiredDate = Convert.ToDateTime(InsertData.RequiredDate),
                ShippedDate = Convert.ToDateTime(InsertData.ShippedDate),
                CompanyName = ShippersList[form_CompanyName].CompanyName,
                Freight = InsertData.Freight,
                ShipCountry = InsertData.ShipCountry,
                ShipCity = InsertData.ShipCity,
                ShipRegion = InsertData.ShipRegion,
                ShipPostalCode = InsertData.ShipPostalCode,
                ShipAddress = InsertData.ShipAddress
            });

            ViewBag.EmployeeName = EmployeeNameList;

            ViewBag.CompanyName = CompanyNameList;
            return View("Index");
        }

        //GET: 刪除訂單動作
        [HttpGet]
        public ActionResult DeleteOrders(int id)
        {
            OrderList.Remove(OrderList.Find(item => ((Models.InsertViewModel)item).OrderID == id));

            ViewBag.EmployeeName = EmployeeNameList;

            ViewBag.CompanyName = CompanyNameList;

            return View("Index");
        }

        //建立假資料
        public void DataSetUp()
        {
            //建立客戶資料
            for (int i = 0; i < 5; i++)
            {
                CustomerList.Add(new Models.Customers
                {
                    CustomerID = i,
                    CompanyName = faker.Company.CompanyName(),
                    ContactName = faker.Name.FullName()
                });
            }

            //建立員工資料
            for (int j = 0; j < 5; j++)
            {
                EmployeeList.Add(new Models.Employees
                {
                    EmployeeID = j,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName()
                });
            }

            //建立運輸公司資料
            for (int k = 0; k < 5; k++)
            {
                ShippersList.Add(new Models.Shippers
                {
                    ShipperID = k,
                    CompanyName = faker.Company.CompanyName(),
                    Phone = faker.Phone.PhoneNumber()
                });
            }

            //建立訂單資料
            for (int x = 0; x < 10; x++)
            {
                var rngDate = faker.Date.Between(Convert.ToDateTime("2017/01/01"), Convert.ToDateTime("2017/12/31"));
                OrderList.Add(new Models.InsertViewModel
                {
                    OrderID = x,
                    ContactName = CustomerList[x%5].ContactName,
                    EmployeeName = EmployeeList[x%5].LastName,
                    OrderDate = rngDate,
                    RequiredDate = rngDate,
                    ShippedDate = rngDate.AddDays(5),
                    CompanyName = ShippersList[x%5].CompanyName,
                    Freight = Decimal.Parse(faker.Commerce.Price(1000, 2000, 0, "")),
                    ShipCountry = faker.Address.Country(),
                    ShipCity = faker.Address.City(),
                    ShipRegion = faker.Address.State(),
                    ShipPostalCode = faker.Address.ZipCode(),
                    ShipAddress = faker.Address.FullAddress()
                });
            }
        }

        //建立負責員工下拉式選單data
        public void SetEmployeeNameList()
        {
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                EmployeeNameList.Add(new SelectListItem()
                {
                    Text = EmployeeList[i].LastName,
                    Value = i.ToString(),
                    Selected = false
                });
            }
        }

        //建立運輸公司名稱下拉式選單data
        public void SetCompanyNameList()
        {
            for (int j = 0; j < CustomerList.Count; j++)
            {
                CompanyNameList.Add(new SelectListItem()
                {
                    Text = ShippersList[j].CompanyName,
                    Value = j.ToString(),
                    Selected = false
                });
            }
        }

        //建立客戶名稱下拉式選單data
        public void SetContactNameList()
        {
            for (int j = 0; j < CustomerList.Count; j++)
            {
                ContactNameList.Add(new SelectListItem()
                {
                    Text = CustomerList[j].ContactName,
                    Value = j.ToString(),
                    Selected = false
                });
            }
        }
    }
}