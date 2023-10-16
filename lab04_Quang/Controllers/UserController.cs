using lab04_Quang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lab04_Quang.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        dbDataClasses1DataContext db = new dbDataClasses1DataContext();
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection, customer c)
        {
            var name = collection["customer_name"];
            var username = collection["username"];
            var password = collection["password"];
            var confirmpassword = collection["confirmpassword"];
            var email = collection["email"];
            var address = collection["address"];
            var numberphone = collection["numberphone"];
            var dob = string.Format("{0:MM/dd/yyyy}", collection["dob"]);

            if(string.IsNullOrEmpty(confirmpassword))
            {
                ViewData["enterpassword"] = "Must enter password to confirm!";
                
            }
            else
            {
                if(!password.Equals(confirmpassword))
                {
                    ViewData["samepassword"] = "Password and confirmation password must be the same";
                }
                else
                {
                    c.customer_name = name;
                    c.username = username;
                    c.password = password;  
                    c.email = email;
                    c.address = address;
                    c.numberphone = numberphone;
                    c.dob = DateTime.Parse(dob);
                    db.customers.InsertOnSubmit(c);
                    db.SubmitChanges();
                    
                    return RedirectToAction("Register");   
                }
            }

            return this.Register();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var username = collection["username"];
            var password = collection["password"];
            customer c = db.customers.SingleOrDefault(n => n.username == username && n.password == password);
            if(c != null)
            {
                ViewBag.Thongbao = "congratulations on successful login";
                Session["User"] = c;
            }
            else
            {
                ViewBag.Thongbao = "Username or password is incorrect";
            }

            return RedirectToAction("Home", "Book");
        }









    }


















}