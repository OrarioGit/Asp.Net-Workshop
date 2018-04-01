using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Asp_Net_Project.Models
{
    public class QueryViewModel
    {
        [DisplayName("訂單編號")]
        public int OrderID { get; set; }

        [DisplayName("客戶名稱")]
        public string ContactName { get; set; }

        [DisplayName("負責員工")]
        public string EmployeeName { get; set; }

        [DisplayName("出貨公司")]
        public string CompanyName { get; set; }

        [DisplayName("訂購日期")]
        public DateTime OrderDate { get; set; }

        [DisplayName("出貨日期")]
        public DateTime ShippedDate { get; set; }

        [DisplayName("需要日期")]
        public DateTime RequiredDate { get; set; }
    }
}