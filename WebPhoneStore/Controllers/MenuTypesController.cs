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
    public class MenuTypesController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: MenuTypes
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
            IQueryable<MenuType> lstTag = db.MenuTypes;
            if (!string.IsNullOrEmpty(tukhoa))
            {
                lstTag = lstTag.Where(p => p.Name.Contains(tukhoa));
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
            return View(lstTag.ToPagedList(pageNumber, pageSize));
        }

        // GET: MenuTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuType menuType = db.MenuTypes.Find(id);
            if (menuType == null)
            {
                return HttpNotFound();
            }
            return View(menuType);
        }

        // GET: MenuTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] MenuType menuType)
        {
            if (ModelState.IsValid)
            {
                db.MenuTypes.Add(menuType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menuType);
        }

        // GET: MenuTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuType menuType = db.MenuTypes.Find(id);
            if (menuType == null)
            {
                return HttpNotFound();
            }
            return View(menuType);
        }

        // POST: MenuTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] MenuType menuType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menuType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuType);
        }

        // GET: MenuTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuType menuType = db.MenuTypes.Find(id);
            if (menuType == null)
            {
                return HttpNotFound();
            }
            return View(menuType);
        }

        // POST: MenuTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            MenuType menuType = db.MenuTypes.Find(id);
            db.MenuTypes.Remove(menuType);
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
