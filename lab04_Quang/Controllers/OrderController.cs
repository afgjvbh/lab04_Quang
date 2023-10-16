using lab04_Quang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lab04_Quang.Controllers
{
    public class OrderController : Controller
    {
        dbDataClasses1DataContext db = new dbDataClasses1DataContext();
        // GET: Order
        public ActionResult Order()
        {
            List<Order> lst = (from a in db.orders 
                               join b in db.orderdetails on a.order_id equals b.order_id
                               join c in db.customers on a.customer_id equals c.customer_id
                               group new { a, b, c } by b.order_id into g
                               select new Order
                               {
                                   orderID = g.First().a.order_id,
                                   customerID = g.First().a.customer_id.ToString(),
                                   customerName = g.First().c.customer_name, 
                                   isShip = g.First().a.isship.Value,
                                   isPayment = g.First().a.ispayment.Value,
                                   deliveryDate = g.First().a.delivery_date.Value,
                                   orderDate = g.First().a.order_date.Value,
                                   total = g.Sum(x => x.b.price)
                               } ).ToList();
            return View(lst);
        }

public ActionResult Detail(int id)
{
    var D_Order = db.orderdetails.FirstOrDefault(m => m.order_id == id);

    if (D_Order != null)
    {
        // Lấy thông tin sách dựa trên book_id từ D_Order
        var book = db.books.FirstOrDefault(b => b.book_id == D_Order.book_id);

        if (book != null)
        {
            ViewBag.BookName = book.book_name; // Lưu tên sách vào ViewBag
        }
    }

    return View(D_Order);
}
    }
}