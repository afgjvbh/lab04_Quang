using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab04_Quang.Models
{
    public class Order
    {
        public int orderID { get; set; }
        public string customerID { get; set; }
        public string customerName { get; set; }
        public bool isShip { get; set; }
        public bool isPayment { get; set; }
        public DateTime deliveryDate { get; set; }
        public DateTime orderDate { get; set; }
        public decimal? total { get; set; }


    }
}