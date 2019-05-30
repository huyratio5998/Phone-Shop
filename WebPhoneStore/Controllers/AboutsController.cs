using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Models;
using PagedList;
using System.IO;
namespace WebPhoneStore.Controllers
{
    public class AboutsController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Abouts
        public ActionResult Index(int? page, string sortOrder, string tukhoa, string CurrentString)
        {
            if (tukhoa != null)
            {
                page = 1;
            }
            else
            {
                tukhoa = CurrentString;
            }
            ViewBag.CurrentString = tukhoa;
            IQueryable<About> lstTag = db.Abouts;
            if (!string.IsNullOrEmpty(tukhoa))
            {
                lstTag = lstTag.Where(p => p.Name.Contains(tukhoa)||p.MetaTitle.Contains(tukhoa)
                ||p.Description.Contains(tukhoa)||p.Detail.Contains(tukhoa)||p.MetaKeyword.Contains(tukhoa)
                ||p.MetaDescription.Contains(tukhoa));
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.MetaTitle = string.IsNullOrEmpty(sortOrder) ? "MetaTitle" : "";
            ViewBag.SortName = string.IsNullOrEmpty(sortOrder) ? "SortName" : "";
            ViewBag.CreateDate = string.IsNullOrEmpty(sortOrder) ? "CreateDate" : "";
            ViewBag.CreateBy = string.IsNullOrEmpty(sortOrder) ? "CreateBy" : "";
            ViewBag.ModifleDate = string.IsNullOrEmpty(sortOrder) ? "ModifleDate" : "";
            ViewBag.ModifileBy = string.IsNullOrEmpty(sortOrder) ? "ModifileBy" : "";
            switch (sortOrder)
            {
                case "SortName":
                    lstTag = lstTag.OrderBy(p => p.Name);
                    break;
                case "MetaTitle":
                    lstTag = lstTag.OrderBy(p => p.MetaTitle);
                    break;
                case "CreateDate":
                    lstTag = lstTag.OrderBy(p => p.CreateDate);
                    break;
                case "CreateBy":
                    lstTag = lstTag.OrderBy(p => p.CreateBy);
                    break;
                case "ModifleDate":
                    lstTag = lstTag.OrderBy(p => p.ModifileDate);
                    break;
                case "ModifileBy":
                    lstTag = lstTag.OrderBy(p => p.ModifileBy);
                    break;
                default:
                    lstTag = lstTag.OrderByDescending(p => p.CreateDate);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstTag.ToPagedList(pageNumber, pageSize));
        }

        // GET: Abouts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // GET: Abouts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Abouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,Description,Image,Detail,CreateDate,CreateBy,ModifileDate,ModifileBy,MetaKeyword,MetaDescription,Status")] About about, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if(file!=null && file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Content/Images/" + filename));
                    about.Image = filename;
                }
                else
                {
                    about.Image = "noImage.png";
                }
                db.Abouts.Add(about);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(about);
        }

        // GET: Abouts/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: Abouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaTitle,Description,Image,Detail,CreateDate,CreateBy,ModifileDate,ModifileBy,MetaKeyword,MetaDescription,Status")] About about,HttpPostedFileBase file,long ? id)
        {
            About objAbout = db.Abouts.Find(id);
            if (ModelState.IsValid)
            {
                if(file!=null&& file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Content/Images/" + filename));
                    about.Image = filename;
                }
                else
                {
                    about.Image = objAbout.Image;
                }
                db.Entry(objAbout).CurrentValues.SetValues(about);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(about);
        }

        // GET: Abouts/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: Abouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            About about = db.Abouts.Find(id);
            db.Abouts.Remove(about);
            db.SaveChanges();
            return RedirectToAction("Index");
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
