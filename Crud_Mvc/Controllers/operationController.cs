using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crud_Mvc.Models;
namespace Crud_Mvc.Controllers
{
    public class operationController : Controller
    {
        UserDbContext db = new UserDbContext();
        // GET: operation
        public ActionResult login()
        {
            //check if cookies file is exist or not
            if (Request.Cookies["mvc"] != null)
            {
                Session.Add("userid", Request.Cookies["mvc"].Values["userid"]);
                return RedirectToAction("profile");
            }
            return View();
        }

        [HttpPost]
        public ActionResult login(user u, bool remberme)
        {
            user s = db.users.Where(n => n.email == u.email && n.password == u.password).FirstOrDefault();
            if (s != null)
            {
                //login
                if (remberme == true)
                {
                    //add cookies
                    HttpCookie co = new HttpCookie("mvc");
                    co.Values.Add("userid", s.id.ToString());
                    co.Values.Add("name", s.name.ToString());
                    co.Expires = DateTime.Now.AddMonths(2);
                    Response.Cookies.Add(co);
                }
                Session.Add("userid", s.id);
                return RedirectToAction("profile");
            }
            else
            {
                ViewBag.status = "incorrect email or password ";
                return View();
            }
        }

        public ActionResult profile()
        {
            int id = int.Parse(Session["userid"].ToString());
            return View(db.users.Find(id));
        }

        public ActionResult Changepassword(int id)
        {
            user user = db.users.Find(id);
            return View(user);
        }
        
        [HttpPost]
        public ActionResult Changepassword(int id,string OldPassword, string NewPassword, string ConfirmNewPassword)
        {
            user user = db.users.Find(id);

            if (user.password == OldPassword)
            {
                if (NewPassword == ConfirmNewPassword)
                {
                    user.name = user.name;
                    user.email = user.email;
                    user.national_id = user.national_id;
                    user.age = user.age;
                    user.password = NewPassword;
                    user.confirm_password = ConfirmNewPassword;
                    db.SaveChanges();
                    return RedirectToAction("profile");
                }
                else
                {
                    ViewBag.status = "Password not match";
                    return View();
                }
            }
            else
            {
                ViewBag.status = "incorrect OldPassword ";
                return View();
            }

        }
        public ActionResult logout()
        {
            Session["userid"] = null;
            HttpCookie c = new HttpCookie("mvc");
            c.Expires = DateTime.Now.AddMonths(-1);
            Response.Cookies.Add(c);
            return RedirectToAction("login");
        }
    }
}