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

using System.IO;
namespace WebPhoneStore.Controllers
{
    public class ContentsController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Contents
        public ActionResult Index(string name, string currentName, int ? categoryID, string sortOrder, int ?page)
        {
            List<CategoryProduct> lstCP = db.CategoryProducts.ToList();
            ViewBag.lstCP = new SelectList(lstCP, "ID", "Name");
            //paging:
            if(!string.IsNullOrEmpty(name))
            {
                page = 1;
            }
            else
            {
                name = currentName;
            }
            ViewBag.CurrentSelect = categoryID;
            ViewBag.CurrentName = name;
            // search
            IQueryable<Content> lstContent = db.Contents;
            if (!string.IsNullOrEmpty(name))
            {
                lstContent = lstContent.Where(p => p.Name.Contains(name) || p.Description.Contains(name));
            }
            if (categoryID != null)
            {
                lstContent = lstContent.Where(p => p.CategoryID == categoryID);
            }
            // sort
            ViewBag.CurrentSort = sortOrder;         
            ViewBag.SortName = string.IsNullOrEmpty(sortOrder) ? "sortName" : "";
            ViewBag.SortDate = string.IsNullOrEmpty(sortOrder) ? "sortDate" : "";
            switch (sortOrder)
            {
                case "sortName":
                    lstContent = lstContent.OrderBy(p => p.Name);
                    break;
                case "sortDate":
                    lstContent = lstContent.OrderBy(p => p.CreateDate);
                    break;
                default:
                    lstContent = lstContent.OrderBy(p => p.ID);
                    break;
            }
            int pageNumber = (page ?? 1);
            int pageSize = 3;
            return View(lstContent.ToPagedList(pageNumber, pageSize));
        }

        // GET: Contents/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // GET: Contents/Create
        public ActionResult Create(Content objContent)
        {
            List<CategoryProduct> lstCP = db.CategoryProducts.ToList();
            ViewBag.lstCP = new SelectList(lstCP, "ID", "Name");
            return View(objContent);
        }

        // POST: Contents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,Description,Image,CategoryID,Detail,Warranty,CreateDate,CreateBy,ModifileDate,ModifileBy,MetaKeyword,MetaDescription,Status,TopHot,ViewCount,Tags")] Content content, HttpPostedFileBase file)
        {
            
            if (ModelState.IsValid)
            {
                if(file!=null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Content/Images/" + fileName));
                    content.Image = fileName;
                }
                else
                {
                    content.Image = "noImage.png";
                }
                content.CreateDate = DateTime.Now.Date;
                db.Contents.Add(content);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(content);
        }

        // GET: Contents/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<CategoryProduct> lstCP = db.CategoryProducts.ToList();
            ViewBag.lstCP = new SelectList(lstCP, "ID", "Name");
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaTitle,Description,Image,CategoryID,Detail,Warranty,CreateDate,CreateBy,ModifileDate,ModifileBy,MetaKeyword,MetaDescription,Status,TopHot,ViewCount,Tags")] Content content, HttpPostedFileBase file, long ? id)
        {
            Content olderContent = db.Contents.Find(id);
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Content/Images/" + fileName));
                    content.Image = fileName;
                }
                else
                {
                    content.Image = olderContent.Image;
                }
                db.Entry(olderContent).CurrentValues.SetValues(content);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(content);
        }

        // GET: Contents/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
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
