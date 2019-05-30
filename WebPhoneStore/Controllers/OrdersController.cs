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
    public class OrdersController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Orders
        public ActionResult Index(int? page, string tukhoa, string currentSearch)
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
            IQueryable<Order> lstOrder = db.Orders;
            if (!string.IsNullOrEmpty(tukhoa))
            {
                lstOrder = lstOrder.Where(p => p.ShipName.Contains(tukhoa) || p.ShipAddress.Contains(tukhoa));
            }
            

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstOrder.OrderByDescending(p=>p.CreateDate).ToPagedList(pageNumber, pageSize));
        }

        // GET: Orders/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
