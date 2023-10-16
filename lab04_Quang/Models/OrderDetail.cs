using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab04_Quang.Models
{
    public class OrderDetailViewModel
    {
        public string BookName { get; set; }
        public decimal TotalForBook { get; set; } // Thêm cột tổng cho từng cuốn sách
    }
}