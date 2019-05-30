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
    public class TagsController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Tags
        public ActionResult Index(int ?page,string sortOrder,string tukhoa,string currentSearch)
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
            IQueryable<Tag> lstTag = db.Tags;
            if (!string.IsNullOrEmpty(tukhoa))
            {
                lstTag = lstTag.Where(p => p.Name.Contains(tukhoa) || p.ID.Contains(tukhoa));
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortID = string.IsNullOrEmpty(sortOrder) ? "sortID" : "";
            ViewBag.SortName = string.IsNullOrEmpty(sortOrder) ? "sortname" : "";
            switch (sortOrder)
            {
                case "sortname":
                    lstTag = lstTag.OrderBy(p => p.Name);
                    break;
                case "sortID":
                    lstTag = lstTag.OrderByDescending(p => p.ID);
                    break;
                default:
                    lstTag = lstTag.OrderBy(p => p.ID);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstTag.ToPagedList(pageNumber,pageSize));
        }

        // GET: Tags/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tag);
        }

        // GET: Tags/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
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
