using lab04_Quang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lab04_Quang.Controllers
{
    public class CartController : Controller
    {
        dbDataClasses1DataContext db = new dbDataClasses1DataContext(); 
        // GET: Cart
        public List<Cart> getCart() 
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;

            if(lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["Cart"] = lstCart;

            }
            return lstCart;
        }
        public ActionResult AddCart(int id, string strURL) 
        {
            List<Cart> lstCart = getCart();
            Cart product = lstCart.Find(n => n.book_id == id);
            if(product == null)
            {
                product = new Cart(id);
                lstCart.Add(product);
                return Redirect(strURL);
            }
            else
            {
                product.iquantity++;
                return Redirect(strURL);
            }

        }

        private int SumQuantity()
        {
            int tsl = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if(lstCart != null)
            {
                tsl = lstCart.Sum(n => n.iquantity);
            }
            return tsl;

        }

        private int sumProductQuantity()
        {
            int tsl = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                tsl = lstCart.Count();
            }
            return tsl;
        }

        private double Total()
        {
            double tt = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                tt = lstCart.Sum(n => n.Total);
            }
            return tt;
        }


        public ActionResult Cart ()
        {
            List<Cart> lstCart = getCart();
            ViewBag.SumQuantity = SumQuantity();
            ViewBag.Total = Total();
            ViewBag.sumProductQuantity = sumProductQuantity();
            return View(lstCart);
        }

        public ActionResult CartPartial()
        {
            ViewBag.SumQuantity = SumQuantity();
            ViewBag.Total = Total();
            ViewBag.sumProductQuantity = sumProductQuantity();
            return PartialView();
        }

        public ActionResult CartDelete(int id)
        {
            List<Cart> lstCart = getCart();
            Cart product = lstCart.SingleOrDefault(n => n.book_id == id);
            if (product != null) 
            {
                lstCart.RemoveAll(n => n.book_id == id);
                return RedirectToAction("Cart");
            }
            return RedirectToAction("Cart");
        }

        public ActionResult CartUpdate(int id, FormCollection collection)
        {
            List<Cart> lstCart = getCart();
            Cart product = lstCart.SingleOrDefault(n => n.book_id == id);
            if (product != null)
            {
                product.iquantity = int.Parse(collection["txtSoLg"].ToString());
            }
            return RedirectToAction("Cart");
        }

        public ActionResult AllCartDelete()
        {
            List<Cart> lstCart = getCart();
            lstCart.Clear();
            return RedirectToAction("Cart");
        }


        [HttpGet]
        public ActionResult PlaceOrder()
        {
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Book");
            }
            List<Cart> lstCart = getCart();

            ViewBag.SumQuantity = SumQuantity();
            ViewBag.Total = Total();
            ViewBag.sumProductQuantity = sumProductQuantity();

            return View(lstCart);
        }
        public ActionResult PlaceOrder(FormCollection collection)
        {
            order dh = new order();
            customer kh = (customer)Session["User"];
            book s = new book();

            List<Cart> gh = getCart();
            var delivery_date = String.Format("{0:MM/dd/yyyy}", collection["delivery_date"]);

            dh.customer_id = kh.customer_id;
            dh.order_date = DateTime.Now;
            dh.delivery_date = DateTime.Parse(delivery_date);
            dh.isship = false;
            dh.ispayment = false;

            db.orders.InsertOnSubmit(dh);
            db.SubmitChanges();

            foreach (var item in gh) 
            {
                orderdetail ctdh = new orderdetail();

                ctdh.order_id = dh.order_id;
                ctdh.book_id = item.book_id;
                ctdh.quantity = item.iquantity;
                ctdh.price = (decimal)item.price;

                s = db.books.Single(n => n.book_id == item.book_id);
                s.quantity_instock -= ctdh.quantity;
                db.SubmitChanges();
                db.orderdetails.InsertOnSubmit(ctdh);
            }

            db.SubmitChanges();
            Session["Cart"] = null;

            return RedirectToAction("ConfirmOrder", "Cart");

        }

        public ActionResult ConfirmOrder()
        {
            return View();
        }


    }
}