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
    public class FeedbacksController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Feedbacks
        public ActionResult Index(string tukhoa, string currentSearch, string sortOrder, int? page)
        {
            if (tukhoa != null)
            {
                page = 1;
            }
            if (tukhoa == null)
            {
                tukhoa = currentSearch;
            }
            ViewBag.CurrentSearch = tukhoa;

            IQueryable<Feedback> lstFb = db.Feedbacks;
            if (tukhoa != null)
            {
                lstFb = lstFb.Where(p => p.Name.Contains(tukhoa) || p.Email.Contains(tukhoa) || p.Content.Contains(tukhoa));
            }
            // sort;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortName = string.IsNullOrEmpty(sortOrder) ? "sortname" : "";
            ViewBag.SortEmail = string.IsNullOrEmpty(sortOrder) ? "sortemail" : "";
            ViewBag.SortDate = string.IsNullOrEmpty(sortOrder) ? "sortdate" : "";
            ViewBag.SortStatus = string.IsNullOrEmpty(sortOrder) ? "sortstatus" : "";
            switch (sortOrder)
            {
                case "sortname":
                    lstFb = lstFb.OrderBy(p => p.Name);
                    break;
                case "sortemail":
                    lstFb = lstFb.OrderBy(p => p.Email);
                    break;
                case "sortdate":
                    lstFb = lstFb.OrderBy(p => p.CreateDate);
                    break;
                case "sortstatus":
                    lstFb = lstFb.OrderBy(p => p.Status);
                    break;
                default:
                    lstFb = lstFb.OrderByDescending(p => p.CreateDate);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(lstFb.ToPagedList(pageNumber, pageSize));
        }

        // GET: Feedbacks/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Phone,Email,Address,Content,CreateDate,Status")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Phone,Email,Address,Content,CreateDate,Status")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
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
