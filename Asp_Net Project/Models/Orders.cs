using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Asp_Net_Project.Models
{
    
    public class Orders
    {
        public int OrderID { get; set;}

        public int CustomerID { get; set;}

        public int EmployeeID { get; set;}

        public DateTime OrderDate { get; set;}

        public DateTime RequiredDate { get; set;}

        public DateTime ShippedDate { get; set;}

        public int ShipperID { get; set;}

        public decimal Freight { get; set;}

        public string ShipAddress { get; set;}

        public string ShipCity { get; set;}

        public string ShipRegion { get; set;}

        public string ShipPostalCode { get; set;}

        public string ShipCountry { get; set;}
    }
}