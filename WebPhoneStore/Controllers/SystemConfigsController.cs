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
    public class SystemConfigsController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: SystemConfigs
        public ActionResult Index(string tukhoa,string currentSearch, string sortOrder, string CurrentSort,int? page)
        {
            //search
            if (tukhoa != null)
            {
                page = 1;
            }
            else
            {
                tukhoa = currentSearch;
            }
            ViewBag.CurrentSearch = tukhoa;
            IQueryable<SystemConfig> lstSys = db.SystemConfigs;
            if (tukhoa != null)
            {
                lstSys = lstSys.Where(p => p.ID.Contains(tukhoa) || p.Name.Contains(tukhoa) || p.Value.Contains(tukhoa)||p.Type.Contains(tukhoa));
            }
            // sort
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortName = string.IsNullOrEmpty(sortOrder) ? "sortName" : "";
            ViewBag.SortType = string.IsNullOrEmpty(sortOrder) ? "sortType" : "";
            ViewBag.SortValue = string.IsNullOrEmpty(sortOrder) ? "sortValue" : "";
            switch (sortOrder)
            {
                case "sortName":
                    lstSys = lstSys.OrderBy(p => p.Name);
                    break;
                case "sortType":
                    lstSys = lstSys.OrderBy(p => p.Type);
                    break;
                case "sortValue":
                    lstSys = lstSys.OrderBy(p => p.Value);
                    break;
                default:
                    lstSys = lstSys.OrderBy(p => p.ID);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstSys.ToPagedList(pageNumber,pageSize));
        }

        // GET: SystemConfigs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemConfig systemConfig = db.SystemConfigs.Find(id);
            if (systemConfig == null)
            {
                return HttpNotFound();
            }
            return View(systemConfig);
        }

        // GET: SystemConfigs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemConfigs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Type,Value,Status")] SystemConfig systemConfig)
        {
            if (ModelState.IsValid)
            {
                db.SystemConfigs.Add(systemConfig);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(systemConfig);
        }

        // GET: SystemConfigs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemConfig systemConfig = db.SystemConfigs.Find(id);
            if (systemConfig == null)
            {
                return HttpNotFound();
            }
            return View(systemConfig);
        }

        // POST: SystemConfigs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Type,Value,Status")] SystemConfig systemConfig)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemConfig).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(systemConfig);
        }

        // GET: SystemConfigs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemConfig systemConfig = db.SystemConfigs.Find(id);
            if (systemConfig == null)
            {
                return HttpNotFound();
            }
            return View(systemConfig);
        }

        // POST: SystemConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SystemConfig systemConfig = db.SystemConfigs.Find(id);
            db.SystemConfigs.Remove(systemConfig);
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
