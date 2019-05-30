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
    public class MenusController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();

        // GET: Menus
        public ActionResult Index(int? page, string sortOrder, string tukhoa, string currentSearch,int ?typeID, int ?SelectedID)
        {
            var lstMenuType = db.MenuTypes.OrderBy(p=>p.Name).ToList();
            ViewBag.lstMenuType = new SelectList(lstMenuType, "ID", "Name");
            // chu y: typeID!=selectedID vi: biến typeID dùng để set xem drop đang chọn gì
            // selectedID : khi typeID có id tìm kiếm page=1; selectid còn dùng để set giá trị cho typeID khi nó rỗng (khi search, sort)
            // selectid: khi đang paging, ...
            if (tukhoa != null|| (typeID != null&&typeID!=SelectedID))
            {
                page = 1;
            }
            if(string.IsNullOrEmpty(tukhoa))
            {
                tukhoa = currentSearch;
            }
            if (typeID == null)
            {
                typeID = SelectedID;
            }
            ViewBag.CurrentSearch = tukhoa;
            ViewBag.SelectedID = typeID;
            IQueryable<Menu> lstMenu = db.Menus;
            if (!string.IsNullOrEmpty(tukhoa))
            {
                lstMenu = lstMenu.Where(p => p.Text.Contains(tukhoa)||p.Target.Contains(tukhoa));
            }
            if (typeID != null){
                lstMenu = lstMenu.Where(p => p.TypeID == typeID);
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortText = string.IsNullOrEmpty(sortOrder) ? "sorttext" : "";
            ViewBag.SortStatus = string.IsNullOrEmpty(sortOrder) ? "sortstatus" : "";
            ViewBag.SortType = string.IsNullOrEmpty(sortOrder) ? "sorttype" : "";
            switch (sortOrder)
            {
                case "sortname":
                    lstMenu = lstMenu.OrderBy(p => p.Text);
                    break;
                case "sortstatus":
                    lstMenu = lstMenu.OrderBy(p => p.Status);
                    break;
                case "sorttype":
                    lstMenu = lstMenu.OrderBy(p => p.TypeID);
                    break;
                default:
                    lstMenu = lstMenu.OrderBy(p => p.DisplayOrder);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstMenu.ToPagedList(pageNumber, pageSize));
        }

        // GET: Menus/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Menus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Text,Link,DisplayOrder,Target,Status,TypeID")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        // GET: Menus/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Text,Link,DisplayOrder,Target,Status,TypeID")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: Menus/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
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
