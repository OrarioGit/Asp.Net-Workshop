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
            
            //建立負責員工data
            List<SelectListItem> EmployeeNameList = new List<SelectListItem>();

            for (int i = 0; i < EmployeeList.Count; i++)
            {
                EmployeeNameList.Add(new SelectListItem()
                {
                    Text = ((Models.Employees)EmployeeList[i]).LastName,
                    Value = i.ToString()
                });
            }

            ViewBag.EmployeeName = EmployeeNameList;


            //建立公司名稱data
            List<SelectListItem> CompanyNameList = new List<SelectListItem>();

            for (int j = 0; j < CustomerList.Count; j++)
            {
                CompanyNameList.Add(new SelectListItem()
                {
                    Text = ((Models.Customers)CustomerList[j]).CompanyName,
                    Value = j.ToString()
                });
            }
            
            ViewBag.CompanyName = CompanyNameList;

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

            //建立負責員工data
            List<SelectListItem> EmployeeNameList = new List<SelectListItem>();

            for (int i = 0; i < EmployeeList.Count; i++)
            {
                EmployeeNameList.Add(new SelectListItem()
                {
                    Text = ((Models.Employees)EmployeeList[i]).LastName,
                    Value = i.ToString()
                });
            }

            ViewBag.EmployeeName = EmployeeNameList;


            //建立公司名稱data
            List<SelectListItem> CompanyNameList = new List<SelectListItem>();

            for (int x = 0; x < CustomerList.Count; x++)
            {
                CompanyNameList.Add(new SelectListItem()
                {
                    Text = ((Models.Customers)CustomerList[x]).CompanyName,
                    Value = x.ToString()
                });
            }

            ViewBag.CompanyName = CompanyNameList;

            return View(insert_model);
        }


        public void DataSetUp()
        {
            for (int i = 0; i < 5; i++)
            {
                var rngDate = faker.Date.Between(Convert.ToDateTime("2017/01/01"), Convert.ToDateTime("2017/12/31"));
                OrderList.Add(new Models.Orders
                {
                    OrderID = i,
                    OrderDate = rngDate,
                    ShippedDate = rngDate.AddDays(5),
                    RequiredDate = rngDate,
                    Freight = Decimal.Parse(faker.Commerce.Price(100, 1000, 0, "")),
                    ShipCountry = faker.Address.Country(),
                    ShipCity = faker.Address.City(),
                    ShipRegion = faker.Address.State(),
                    ShipPostalCode = faker.Address.ZipCode(),
                    ShipAddress = faker.Address.FullAddress()
                });

                CustomerList.Add(new Models.Customers
                {
                    CustomerID = i,
                    CompanyName = faker.Company.CompanyName(),
                    ContactName = faker.Name.FullName()
                });

                EmployeeList.Add(new Models.Employees
                {
                    EmployeeID = i,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName()
                });

                ShippersList.Add(new Models.Shippers
                {
                    ShipperID = i,
                    CompanyName = faker.Company.CompanyName(),
                    Phone = faker.Phone.PhoneNumber()
                });
            }
        }
    }
}