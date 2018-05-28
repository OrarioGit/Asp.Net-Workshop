using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Asp_Net_Project.Models
{
    public class InsertViewModel
    {
        [DisplayName("訂單編號")]
        public int OrderID { get; set; }

        [DisplayName("*客戶名稱")]
        [Required()]
        public string ContactName { get; set; }

        [DisplayName("*負責員工名稱")]
        [Required()]
        public string EmployeeName { get; set; }

        [DisplayName("*訂單日期")]
        [Required()]
        public DateTime OrderDate { get; set; }

        [DisplayName("*需要日期")]
        [Required()]
        public DateTime RequiredDate { get; set; }

        [DisplayName("出貨日期")]
        public DateTime ShippedDate { get; set; }

        [DisplayName("出貨公司名稱")]
        public string CompanyName { get; set; }

        [DisplayName("*運費")]
        public decimal Freight { get; set; }

        [DisplayName("出貨國家")]
        public string ShipCountry { get; set; }

        [DisplayName("出貨城市")]
        public string ShipCity { get; set; }

        [DisplayName("出貨地區")]
        public string ShipRegion { get; set; }

        [DisplayName("郵遞區號")]
        public string ShipPostalCode { get; set; }

        [DisplayName("出貨地址")]
        public string ShipAddress { get; set; }

        [DisplayName("產品名稱名稱")]
        public string ProductName { get; set; }
    }
}