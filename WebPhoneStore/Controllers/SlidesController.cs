using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Models;
using System.IO;
namespace WebPhoneStore.Controllers
{
    public class SlidesController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Slides
        public ActionResult Index(string tukhoa, string currentSearch, string sortOrder, int ?page)
        {
            if (tukhoa != null)
            {
                page = 1;
            }
            else
            {
                tukhoa = currentSearch;
            }
            ViewBag.CurrentSearch = tukhoa;
            //
            IQueryable<Slide> lstSlide = db.Slides;
            if (tukhoa != null)
            {
                lstSlide = lstSlide.Where(p => p.Description.Contains(tukhoa)||p.ModifileBy.Contains(tukhoa));
            }
            //sort
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortDes = string.IsNullOrEmpty(sortOrder) ? "sortdes" : "";
            ViewBag.SortCD = string.IsNullOrEmpty(sortOrder) ? "SortCD" : "";
            ViewBag.CreateBy = string.IsNullOrEmpty(sortOrder) ? "CreateBy" : "";
            ViewBag.ModifileD = string.IsNullOrEmpty(sortOrder) ? "ModifileD" : "";
            ViewBag.ModifileBy = string.IsNullOrEmpty(sortOrder) ? "ModifileBy" : "";
            ViewBag.SortStatus = string.IsNullOrEmpty(sortOrder) ? "SortStatus" : "";
            switch (sortOrder)
            {
                case "sortdes":
                    lstSlide = lstSlide.OrderBy(p => p.Description);
                    break;
                case "SortCD":
                    lstSlide = lstSlide.OrderBy(p => p.CreateDate);
                    break;
                case "CreateBy":
                    lstSlide = lstSlide.OrderBy(p => p.CreateBy);
                    break;
                case "ModifileD":
                    lstSlide = lstSlide.OrderBy(p => p.ModifileDate);
                    break;
                case "ModifileBy":
                    lstSlide = lstSlide.OrderBy(p => p.ModifileBy);
                    break;
                case "SortStatus":
                    lstSlide = lstSlide.OrderBy(p => p.Status);
                    break;
                default:
                    lstSlide = lstSlide.OrderBy(p => p.ID);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstSlide.ToPagedList(pageNumber, pageSize));
        }

        // GET: Slides/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slides.Find(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }

        // GET: Slides/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Slides/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Image,DisplayOrder,Link,Description,CreateDate,CreateBy,ModifileDate,ModifileBy,Status")] Slide slide, HttpPostedFileBase Upfile)
        {
            if (ModelState.IsValid)
            {
                if(Upfile!=null && Upfile.ContentLength > 0)
                {
                    string filename = Path.GetFileName(Upfile.FileName);
                    Upfile.SaveAs(Server.MapPath("~/Content/Images/" + filename));
                    slide.Image = filename;
                }
                else
                {
                    slide.Image = "noImage.png";
                }
                db.Slides.Add(slide);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slide);
        }

        // GET: Slides/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slides.Find(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Image,DisplayOrder,Link,Description,CreateDate,CreateBy,ModifileDate,ModifileBy,Status")] Slide slide, HttpPostedFileBase Upfile,long ?id)
        {
            var slideOlder = db.Slides.Find(id);
            if (ModelState.IsValid)
            {
                if (Upfile != null && Upfile.ContentLength > 0)
                {
                    string filename = Path.GetFileName(Upfile.FileName);
                    Upfile.SaveAs(Server.MapPath("~/Content/Images/" + filename));
                    slide.Image = filename;
                }
                else
                {
                    slide.Image = slideOlder.Image;
                }
                db.Entry(slideOlder).CurrentValues.SetValues(slide);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slide);
        }

        // GET: Slides/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slides.Find(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }

        // POST: Slides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Slide slide = db.Slides.Find(id);
            db.Slides.Remove(slide);
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
