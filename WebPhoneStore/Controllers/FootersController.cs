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
    public class FootersController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Footers
        public ActionResult Index(int? page, string sortOrder, string tukhoa, string currentSearch)
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
            IQueryable<Footer> lstFooter = db.Footers;
            if (!string.IsNullOrEmpty(tukhoa))
            {
                lstFooter = lstFooter.Where(p => p.Content.Contains(tukhoa) || p.ID.Contains(tukhoa));
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortID = string.IsNullOrEmpty(sortOrder) ? "sortID" : "";            
            ViewBag.SortStatus = string.IsNullOrEmpty(sortOrder) ? "sortstatus" : "";
            switch (sortOrder)
            {                
                case "sortID":
                    lstFooter = lstFooter.OrderBy(p => p.ID);
                    break;
                case "sortstatus":
                    lstFooter = lstFooter.OrderBy(p => p.Status);
                    break;
                default:
                    lstFooter = lstFooter.OrderByDescending(p => p.ID);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstFooter.ToPagedList(pageNumber, pageSize));
        }

        // GET: Footers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footers.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // GET: Footers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Footers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Content,Status")] Footer footer)
        {
            if (ModelState.IsValid)
            {
                db.Footers.Add(footer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(footer);
        }

        // GET: Footers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footers.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // POST: Footers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,Status")] Footer footer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(footer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(footer);
        }

        // GET: Footers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footers.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // POST: Footers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Footer footer = db.Footers.Find(id);
            db.Footers.Remove(footer);
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
