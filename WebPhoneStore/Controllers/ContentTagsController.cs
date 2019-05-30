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

namespace WebPhoneStore.Controllers
{
    public class ContentTagsController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: ContentTags
        public ActionResult Index(string tukhoa, string currentSearch, int ? page, string sortOrder)
        {
            // search
            if (tukhoa != null)
            {
                page = 1;
            }
            else
            {
                tukhoa = currentSearch;
            }
            ViewBag.CurrentFilter = tukhoa;
            IQueryable<ContentTag> lstCT = db.ContentTags;
            if (tukhoa != null)
            {
                lstCT = lstCT.Where(p => p.TagID.Contains(tukhoa) || p.ContentID.ToString().Contains(tukhoa));
            }
            // sort
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortContent = string.IsNullOrEmpty(sortOrder) ? "sortcontent" : "";
            ViewBag.SortTag = sortOrder == "sorttag" ? "sortdestag" : "sorttag";
            switch (sortOrder)
            {
                case "sortcontent":
                    lstCT = lstCT.OrderBy(p => p.ContentID);
                    break;
                case "sorttag":
                    lstCT = lstCT.OrderBy(p => p.TagID);
                    break;
                case "sortdestag":
                    lstCT = lstCT.OrderByDescending(p => p.TagID);
                    break;
                default:
                    lstCT = lstCT.OrderByDescending(p => p.ContentID);
                    break;

            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstCT.ToPagedList(pageNumber,pageSize));
        }

        // GET: ContentTags/Details/5
        public ActionResult Details(long ? idC,string idT)
        {
            if (string.IsNullOrEmpty(idT) || idC==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentTag contentTag = db.ContentTags.Find(idC,idT);
            if (contentTag == null)
            {
                return HttpNotFound();
            }
            return View(contentTag);
        }

        // GET: ContentTags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContentTags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContentID,TagID")] ContentTag contentTag)
        {
            if (ModelState.IsValid)
            {
                db.ContentTags.Add(contentTag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contentTag);
        }

        // GET: ContentTags/Edit/5
        public ActionResult Edit(long? idC, string idT)
        {
            if (string.IsNullOrEmpty(idT) || idC == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentTag contentTag = db.ContentTags.Find(idC,idT);
            if (contentTag == null)
            {
                return HttpNotFound();
            }
            return View(contentTag);
        }

        // POST: ContentTags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContentID,TagID")] ContentTag contentTag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contentTag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contentTag);
        }

        // GET: ContentTags/Delete/5
        public ActionResult Delete(long? idC, string idT)
        {
            if (string.IsNullOrEmpty(idT) || idC == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentTag contentTag = db.ContentTags.Find(idC,idT);
            if (contentTag == null)
            {
                return HttpNotFound();
            }
            return View(contentTag);
        }

        // POST: ContentTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long? idC, string idT)
        {
            ContentTag contentTag = db.ContentTags.Find(idC,idT);
            db.ContentTags.Remove(contentTag);
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
