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
    public class OrderDetailsController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: OrderDetails
        public ActionResult Index(int? page, int ?tukhoa, int ?currentSearch, long ID)
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
            IQueryable<OrderDetail> lstOrder = db.OrderDetails;
            
            lstOrder = lstOrder.Where(p => p.OrderID == ID);
            if (tukhoa!=null)
            {
                lstOrder = lstOrder.Where(p => p.Quantity==tukhoa||p.Price==tukhoa);
            }
            ViewBag.lstProduct = db.Products.ToList();
            ViewBag.OrderID = ID;
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstOrder.OrderBy(p => p.ProductID).ToPagedList(pageNumber, pageSize));
        }
            
       
             
        public ActionResult Delete(long? productID, long? orderID)
        {
            OrderDetail orderDetail = db.OrderDetails.Where(p => p.OrderID == orderID && p.ProductID == productID).FirstOrDefault();
            if (orderDetail != null)
            {
                db.OrderDetails.Remove(orderDetail);
                db.SaveChanges();
            }
            return RedirectToAction("Index",new { ID=orderID});
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
