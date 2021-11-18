using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crud_Mvc.Models;
using PagedList;

namespace Crud_Mvc.Controllers
{
    public class UserController : Controller
    {
        UserDbContext db = new UserDbContext(); 
        // GET: User
        public ActionResult Index(int? pageno, string sortedby, string search)
        {
            if (Session["userid"] != null)
            {
                int no = pageno == null ? 1 : pageno.Value;

                var users = db.users.OrderBy(n => n.id);
                if (!String.IsNullOrEmpty(search))
                {
                    users = db.users.Where(n => n.name.Contains(search)).OrderBy(n => n.id);
                }

                ViewBag.sort = sortedby;
                ViewBag.search = search;
                switch (sortedby)
                {
                    case "name":
                        users = users.OrderBy(n => n.name);
                        break;
                    case "email":
                        users = users.OrderBy(n => n.email);
                        break;
                    case "age":
                        users = users.OrderBy(n => n.age);
                        break;
                    default:
                        break;

                }
                return View(users.ToPagedList(no, 3));
            }
            else
            {
                return RedirectToAction("login", "operation");
            }
        }

        public ActionResult create()
        {
            user user = new user();
            user.skills = db.skills.ToList();
            return View(user);
        }

        [HttpPost]
        public ActionResult Create(user usr, HttpPostedFileBase Photo, HttpPostedFileBase cv)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    //save file
                    Photo.SaveAs(Server.MapPath($"~/Attach/{Photo.FileName}"));
                    //edit obj with file path
                    usr.photo = Photo.FileName;
                }
                if (cv != null)
                {
                    //save file
                    cv.SaveAs(Server.MapPath($"~/Attach/{cv.FileName}"));
                    //edit obj with file path
                    usr.cv = cv.FileName;
                }
                List<skill> skills = usr.skills.Where(n => n.ischecked).ToList();
                usr.skills.Clear();
                db.users.Add(usr);
                foreach (skill item in skills)
                {
                    db.skills.Attach(item);
                    usr.skills.Add(item);
                }
                db.SaveChanges();
                return RedirectToAction("index");

            }
            else
            {
                return View();
            }

        }

        public ActionResult edit(int id)
        {
            user user = db.users.Find(id);
            ViewBag.dept = db.skills.ToList();
            return View(user);
        }

        [HttpPost]
        public ActionResult edit(user s, HttpPostedFileBase photo, HttpPostedFileBase cv)
        {
            user us = db.users.Find(s.id);
            if (photo != null)
            {
                photo.SaveAs(Server.MapPath($"~/attach/{photo.FileName}"));
                us.photo = photo.FileName;
            }
            if (cv != null)
            {
                cv.SaveAs(Server.MapPath($"~/attach/{cv.FileName}"));
                us.cv = cv.FileName;
            }
            us.name = s.name;
            us.email = s.email;
            us.national_id = s.national_id;
            us.age = s.age;
            us.password = s.password;
            us.confirm_password = s.confirm_password;
            db.SaveChanges();
            return RedirectToAction("index");

        }

        public ActionResult delete(int id)
        {
            user s = db.users.Find(id);
            foreach (var item in s.news.ToList())
            {
                news news = db.news.Find(item.id);
                db.news.Remove(news);
            }
            db.users.Remove(s);
            db.SaveChanges();
            return RedirectToAction("logout", "operation");
        }
    }
}