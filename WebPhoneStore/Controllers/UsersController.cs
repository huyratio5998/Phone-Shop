using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Models;
using WebPhoneStore.Common;
namespace WebPhoneStore.Controllers
{
    public class UsersController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Users
        public ActionResult Index(string name, string currentName, string sortOrder, int? page)
        {            
            //paging:
            if (!string.IsNullOrEmpty(name))
            {
                page = 1;
            }
            else
            {
                name = currentName;
            }            
            ViewBag.CurrentName = name;
            // search
            IQueryable<User> lstUser = db.Users;
            if (!string.IsNullOrEmpty(name))
            {
                lstUser = lstUser.Where(p => p.Name.Contains(name) || p.UserName.Contains(name));
            }
           
            // sort
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortName = string.IsNullOrEmpty(sortOrder) ? "sortName" : "";
            ViewBag.SortUserName = string.IsNullOrEmpty(sortOrder) ? "sortUserName" : "";
            ViewBag.SortDate = string.IsNullOrEmpty(sortOrder) ? "sortDate" : "";
            ViewBag.SortDate1 = string.IsNullOrEmpty(sortOrder) ? "sortDate1" : "";
            switch (sortOrder)
            {
                case "sortName":
                    lstUser = lstUser.OrderBy(p => p.Name);
                    break;
                case "sortUserName":
                    lstUser = lstUser.OrderBy(p => p.UserName);
                    break;
                case "sortDate":
                    lstUser = lstUser.OrderByDescending(p => p.CreateDate);
                    break;
                case "sortDate1":
                    lstUser = lstUser.OrderBy(p => p.ModifileDate);
                    break;
                default:
                    lstUser = lstUser.OrderBy(p => p.ID);
                    break;
            }
            int pageNumber = (page ?? 1);
            int pageSize = 3;
            return View(lstUser.ToPagedList(pageNumber, pageSize));
        }

        // GET: Users/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AvatarName,UserName,Password,Name,Address,Email,Phone,CreateDate,CreateBy,ModifileDate,ModifileBy,Status")] User user, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Content/Images/" + fileName));
                    user.AvatarName = fileName;
                }
                else
                {
                    user.AvatarName = "noImage.png";
                }
                user.Password = Encryptor.MD5Hash(user.Password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AvatarName,UserName,Password,Name,Address,Email,Phone,CreateDate,CreateBy,ModifileDate,ModifileBy,Status")] User user, HttpPostedFileBase file, long? ID)
        {
            User objUser = db.Users.Find(ID);
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Content/Images/" + fileName));
                    user.AvatarName = fileName;
                }
                else
                {
                    user.AvatarName = objUser.AvatarName;
                }
                if (objUser != null)
                {
                    if(objUser.Password!=Encryptor.MD5Hash(user.Password)) user.Password = Encryptor.MD5Hash(user.Password);
                    db.Entry(objUser).CurrentValues.SetValues(user);
                }
                //db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
