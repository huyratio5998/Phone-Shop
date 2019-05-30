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

namespace WebPhoneStore.Controllers
{
    public class CategoriesController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Categories
        public ActionResult Index(string tukhoa, string CurrentString, string sortOrder ,int ?page)
        {
            //paging
            if (tukhoa != null)
            {
                page = 1;
            }
            if (tukhoa == null)
            {
                tukhoa = CurrentString;
            }
           
            ViewBag.CurrentString = tukhoa;
           
            IQueryable<Category> lstC = db.Categories;

            if (!string.IsNullOrEmpty(tukhoa))
            {
                lstC = lstC.Where(p => p.Name.Contains(tukhoa) || p.MetaData.Contains(tukhoa) || p.SeoTitle.Contains(tukhoa) || p.MetaKeyword.Contains(tukhoa));
            }
            
            
            //sort
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortName= string.IsNullOrEmpty(sortOrder) ? "sortName" : "";
            ViewBag.SeoTitle = string.IsNullOrEmpty(sortOrder) ? "sortST" : "";
            ViewBag.CreateDate = string.IsNullOrEmpty(sortOrder) ? "CD" : "";
            ViewBag.CreateBy = string.IsNullOrEmpty(sortOrder) ? "CB" : "";
            ViewBag.ModifileDate = string.IsNullOrEmpty(sortOrder) ? "MD" : "";
            ViewBag.ModifileBy = string.IsNullOrEmpty(sortOrder) ? "MB" : "";
            switch (sortOrder)
            {

                case "sortName":
                    lstC = lstC.OrderBy(p => p.Name);
                    break;
                case "sortST":
                    lstC = lstC.OrderBy(p => p.SeoTitle);
                    break;
                case "CD":
                    lstC = lstC.OrderBy(p => p.CreateDate);
                    break;
                case "CB":
                    lstC = lstC.OrderBy(p => p.CreateBy);
                    break;
                case "MD":
                    lstC = lstC.OrderBy(p => p.ModifileDate);
                    break;
                case "MB":
                    lstC = lstC.OrderBy(p => p.ModifileBy);
                    break;
                default:
                    lstC = lstC.OrderBy(p => p.ID);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstC.ToPagedList(pageNumber,pageSize));
        }

        // GET: Categories/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,MetaData,ParentID,DisplayOrder,SeoTitle,CreateDate,CreateBy,ModifileDate,ModifileBy,MetaKeyword,MetaDescription,Status,ShowOnHome")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaData,ParentID,DisplayOrder,SeoTitle,CreateDate,CreateBy,ModifileDate,ModifileBy,MetaKeyword,MetaDescription,Status,ShowOnHome")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
