using lab04_Quang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lab04_Quang.Controllers
{
    public class StatisticController : Controller
    {
        dbDataClasses1DataContext db = new dbDataClasses1DataContext();
        // GET: Statistic
        public ActionResult StatisticBook()
        {
            List<SumSaleQuantity> lst = (from a in db.books 
                                         join b in db.orderdetails on a.book_id equals b.book_id 
                                         group b by b.book_id 
                                         into g select 
               new SumSaleQuantity { bookname = g.First().book.book_name, sumQuantity = g.Sum(x => x.quantity) }).ToList();
                return View(lst);
        }
    }
}