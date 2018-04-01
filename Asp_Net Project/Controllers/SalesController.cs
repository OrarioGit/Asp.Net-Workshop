using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asp_Net_Project.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult Index()
        {
            Models.QueryViewModel QueryModel = new Models.QueryViewModel();

            //建立負責員工data
            List<SelectListItem> EmployeeNameList = new List<SelectListItem>();

            EmployeeNameList.Add(new SelectListItem()
            {
                Text = "Andy",
                Value = "0"
            });
            EmployeeNameList.Add(new SelectListItem()
            {
                Text = "Jack",
                Value = "1"
            });


            ViewBag.EmployeeName = EmployeeNameList;

            //建立公司名稱data
            List<SelectListItem> CompanyNameList = new List<SelectListItem>();

            CompanyNameList.Add(new SelectListItem()
            {
                Text = "Company A",
                Value = "0"
            });
            CompanyNameList.Add(new SelectListItem()
            {
                Text = "Company B",
                Value = "1"
            });


            ViewBag.CompanyName = CompanyNameList;

            return View(QueryModel);
        }

        //新增訂單
        public ActionResult InsertOrders()
        {
            Models.InserViewModel insert_model = new Models.InserViewModel();

            //建立客戶名稱data
            List<SelectListItem> ContactNameList = new List<SelectListItem>();

            ContactNameList.Add(new SelectListItem()
            {
                Text =  "Tomy",
                Value = "0"
            });
            ContactNameList.Add(new SelectListItem()
            {
                Text = "Vicky",
                Value = "1"
            });


            ViewBag.ContactName = ContactNameList;

            //建立負責員工data
            List<SelectListItem> EmployeeNameList = new List<SelectListItem>();

            EmployeeNameList.Add(new SelectListItem()
            {
                Text = "Andy",
                Value = "0"
            });
            EmployeeNameList.Add(new SelectListItem()
            {
                Text = "Jack",
                Value = "1"
            });


            ViewBag.EmployeeName = EmployeeNameList;

            //建立公司名稱data
            List<SelectListItem> CompanyNameList = new List<SelectListItem>();

            CompanyNameList.Add(new SelectListItem()
            {
                Text = "Company A",
                Value = "0"
            });
            CompanyNameList.Add(new SelectListItem()
            {
                Text = "Company B",
                Value = "1"
            });


            ViewBag.CompanyName = CompanyNameList;

            return View(insert_model);
        }
    }
}