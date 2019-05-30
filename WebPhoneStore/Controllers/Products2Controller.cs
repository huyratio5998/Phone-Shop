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
using System.IO;
namespace WebPhoneStore.Controllers
{
    public class Products2Controller : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Products2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">ten textbox searchname</param>
        /// <param name="code">textbox searchcode</param>
        /// <param name="sortOrder">dieu kien sort</param>
        /// <param name="currentName">ten hien tai, khi chuyen trang, khi sort va tim kiem</param>
        /// <param name="currentCode">code hien tai, khi chuyen trang, khi sort va tim kiem</param>
        /// <param name="categoryID">loai sp hien tai khi chuyen trang, khi sort va tim kiem</param>
        /// <param name="page">so trang</param>
        /// <returns></returns>
        public ActionResult Index(string name, string code, string sortOrder, string currentName, string currentCode,int ?categoryID, int ? page)
        {
          
            var lstCP = db.CategoryProducts.ToList();
            ViewBag.ProductCategory = new SelectList(lstCP, "ID", "Name");
            //paging + search
            if (name != null || code != null)
            {
                page = 1;
            }
            if(string.IsNullOrEmpty(name))
            {
                name = currentName;
            }
            if (string.IsNullOrEmpty(code))
            {
                code = currentCode;
            }
            ViewBag.CurrentName = name;
            ViewBag.CurrentCode = code;
            ViewBag.CurrentSelect = categoryID;
            IQueryable<Product> lstProduct = db.Products;
            if (!string.IsNullOrEmpty(name))
            {
                lstProduct = lstProduct.Where(p => p.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                lstProduct = lstProduct.Where(p => p.Code.Contains(code));
            }
            if (categoryID != null)
            {
                lstProduct = lstProduct.Where(p => p.CategoryID == categoryID);
            }
            // sort
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortName=string.IsNullOrEmpty(sortOrder)? "sortName": "";
            ViewBag.SortCode = string.IsNullOrEmpty(sortOrder) ? "sortCode" : "";
            switch (sortOrder)
            {
                case "sortName":
                    lstProduct = lstProduct.OrderBy(p => p.Name);
                    break;
                case "sortCode":
                    lstProduct = lstProduct.OrderBy(p => p.Code);
                    break;
                default:
                    lstProduct = lstProduct.OrderBy(p => p.ID);
                    break;
            }
            //
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            //topagedlist phai order truoc moi paging duoc
            return View(lstProduct.ToPagedList(pageNumber,pageSize));
        }

        // GET: Products2/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products2/Create
        public ActionResult Create()
        {
           List<CategoryProduct> lstCP = db.CategoryProducts.ToList();
            ViewBag.lstCP = new SelectList(lstCP, "ID", "Name");
            return View();
        }

        // POST: Products2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Code,MetaTitle,Description,Image,MoreImages,Price,PromotionPrice,IncludeVAT,Quantity,CategoryID,Detail,Warranty,CreateDate,CreateBy,ModifileDate,ModifileBy,MetaKeyword,MetaDescription,Status,TopHot,ViewCount")] Product product,HttpPostedFileBase Upfile)
        {
            if(Upfile!=null && Upfile.ContentLength > 0)
            {
                string filename = Path.GetFileName(Upfile.FileName);
                Upfile.SaveAs(Server.MapPath("~/Content/Images/" + filename));
                product.Image = filename;
            }
            
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products2/Edit/5
        public ActionResult Edit(long? id)
        {
            List<CategoryProduct> lstCP = db.CategoryProducts.ToList();
            ViewBag.lstCP = new SelectList(lstCP, "ID", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Code,MetaTitle,Description,Image,MoreImages,Price,PromotionPrice,IncludeVAT,Quantity,CategoryID,Detail,Warranty,CreateDate,CreateBy,ModifileDate,ModifileBy,MetaKeyword,MetaDescription,Status,TopHot,ViewCount")] Product product, HttpPostedFileBase Upfile, int id)
        {
            Product olderProduct = db.Products.Find(id);
            if(Upfile!=null && Upfile.ContentLength > 0)
            {
                string filename = Path.GetFileName(Upfile.FileName);
                Upfile.SaveAs(Server.MapPath("~/Content/Images/" + filename));
                product.Image = filename;
            }
            else
            {
                product.Image = olderProduct.Image;
            }            
           
            if (ModelState.IsValid)
            {
                db.Entry(olderProduct).CurrentValues.SetValues(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products2/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
