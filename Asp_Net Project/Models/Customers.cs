using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Asp_Net_Project.Models
{
    public class Customers
    {
        public int CustomerID { get; set; }

        public string CompanyName { get; set; }

        [DisplayName("客戶名稱")]
        public string ContactName { get; set;}

        public string Address { get; set;}
    }
}