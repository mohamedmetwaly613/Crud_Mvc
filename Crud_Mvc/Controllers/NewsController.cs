using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Crud_Mvc.Models;
using PagedList;

namespace Crud_Mvc.Controllers 
{
    public class NewsController : Controller
    {
        private UserDbContext db = new UserDbContext();

        public ActionResult Index(int? pageno, string sortedby, string search)
        {
            int no = pageno == null ? 1 : pageno.Value;

            var news1 = db.news.OrderBy(n => n.id);
            if (!String.IsNullOrEmpty(search))
            {
                news1 = db.news.Where(n => n.title.Contains(search)).OrderBy(n => n.id);
            }

            ViewBag.sort = sortedby;
            ViewBag.search = search;
            switch (sortedby)
            {
                case "title":
                    news1 = news1.OrderBy(n => n.title);
                    break;
                case "datetime":
                    news1 = news1.OrderBy(n => n.datetime);
                    break;
                default:
                    break;
            }
            return View(news1.ToPagedList(no, 3));
        }

        public ActionResult catalog()
        {
            List<news> news = db.news.OrderBy(n => n.id).ToList();
            ViewBag.catalog_id = new SelectList(db.catalogs, "id", "name");
            return View(news);
        }

        public ActionResult newsbycatalog(int? catalog_id)
        {
            List<news> news = new List<news>();

            if (catalog_id != null)
            {
                news = db.news.Where(n => n.catalog_id == catalog_id).ToList();
            }
            else
            {
                news = db.news.OrderBy(n => n.id).ToList();
            }
            return PartialView(news);
        }

        public ActionResult allcatalog()
        {
            ViewBag.cat = db.catalogs.ToList();

            return PartialView();
        }

        public ActionResult allcatalogs(int id)
        {
            List<news> news = db.news.Where(n => n.catalog_id==id).ToList();

            return View(news);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            ViewBag.catalog_id = new SelectList(db.catalogs, "id", "name");
            //ViewBag.user_id = new SelectList(db.users, "id", "name");
            int id = int.Parse(Session["userid"].ToString());
            List<user> s = db.users.Where(n => n.id == id).ToList();
            //ViewBag.user_id = s.name;
            ViewBag.user_id = new SelectList(s, "id", "name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,bref,desc,datetime,user_id,catalog_id")] news news, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    //save file
                    photo.SaveAs(Server.MapPath($"~/attach/{photo.FileName}"));
                    //edit obj with file path
                    news.photo = photo.FileName;
                }
                db.news.Add(news);
                db.SaveChanges();
                return RedirectToAction("profile", "operation");
            }

            ViewBag.catalog_id = new SelectList(db.catalogs, "id", "name", news.catalog_id);
            ViewBag.user_id = new SelectList(db.users, "id", "name", news.user_id);
            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.catalog_id = new SelectList(db.catalogs, "id", "name", news.catalog_id);
            ViewBag.user_id = new SelectList(db.users, "id", "name", news.user_id);
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,bref,desc,datetime,user_id,catalog_id")] news news, HttpPostedFileBase photo)
        {
            if (photo != null)
            {
                photo.SaveAs(Server.MapPath($"~/attach/{photo.FileName}"));
                news.photo = photo.FileName;
            }
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("profile", "operation");
            }
            ViewBag.catalog_id = new SelectList(db.catalogs, "id", "name", news.catalog_id);
            ViewBag.user_id = new SelectList(db.users, "id", "name", news.user_id);
            return View(news);
        }

        public ActionResult Delete(int id)
        {
            news news = db.news.Find(id);
            db.news.Remove(news);
            db.SaveChanges();
            return RedirectToAction("profile", "operation");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
