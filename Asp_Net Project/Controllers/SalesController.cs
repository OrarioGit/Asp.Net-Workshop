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
        static List<object> OrderList = new List<object>();
        static List<object> EmployeeList = new List<object>();
        static List<object> CustomerList = new List<object>();
        static List<object> ShippersList = new List<object>();
        
        // GET: Sales
        public ActionResult Index()
        {
            if (OrderList.Count == 0)
            {
                DataSetUp();
            }
            

            Models.QueryViewModel QueryModel = new Models.QueryViewModel();
            
            
            ViewBag.EmployeeName = SetEmployeeNameList();

            ViewBag.CompanyName = SetCompanyNameList();

            return View(QueryModel);
        }

        //新增訂單
        public ActionResult InsertOrders()
        {
            if (OrderList.Count == 0)
            {
                DataSetUp();
            }

            Models.InserViewModel insert_model = new Models.InserViewModel();

            //建立客戶名稱data
            List<SelectListItem> ContactNameList = new List<SelectListItem>();

            for (int j = 0; j < CustomerList.Count; j++)
            {
                ContactNameList.Add(new SelectListItem()
                {
                    Text = ((Models.Customers)CustomerList[j]).ContactName,
                    Value = j.ToString()
                });
            }

            ViewBag.ContactName = ContactNameList;

            ViewBag.EmployeeName = SetEmployeeNameList();

            ViewBag.CompanyName = SetCompanyNameList();

            return View(insert_model);
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
                OrderList.Add(new Models.InserViewModel
                {
                    OrderID = x,
                    ContactName = ((Models.Customers)CustomerList[x%5]).ContactName,
                    EmployeeName = ((Models.Employees)EmployeeList[x%5]).LastName,
                    OrderDate = rngDate,
                    RequiredDate = rngDate,
                    ShippedDate = rngDate.AddDays(5),
                    CompanyName = ((Models.Shippers)ShippersList[x%5]).CompanyName,
                    Freight = Decimal.Parse(faker.Commerce.Price(100, 1000, 0, "")),
                    ShipCountry = faker.Address.Country(),
                    ShipCity = faker.Address.City(),
                    ShipRegion = faker.Address.State(),
                    ShipPostalCode = faker.Address.ZipCode(),
                    ShipAddress = faker.Address.FullAddress()
                });
            }
        }

        //建立負責員工下拉式選單data
        public List<SelectListItem> SetEmployeeNameList()
        {
            
            List<SelectListItem> EmployeeNameList = new List<SelectListItem>();

            for (int i = 0; i < EmployeeList.Count; i++)
            {
                EmployeeNameList.Add(new SelectListItem()
                {
                    Text = ((Models.Employees)EmployeeList[i]).LastName,
                    Value = i.ToString()
                });
            }

            return EmployeeNameList;
        }

        //建立公司名稱下拉式選單data
        public List<SelectListItem> SetCompanyNameList()
        {
            
            List<SelectListItem> CompanyNameList = new List<SelectListItem>();

            for (int j = 0; j < CustomerList.Count; j++)
            {
                CompanyNameList.Add(new SelectListItem()
                {
                    Text = ((Models.Customers)CustomerList[j]).CompanyName,
                    Value = j.ToString()
                });
            }

            return CompanyNameList;
        }
    }
}