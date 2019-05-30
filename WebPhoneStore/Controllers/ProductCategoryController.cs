using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebPhoneStore.Models;
namespace WebPhoneStore.Controllers
{
    public class ProductCategoryController : Controller
    {
        // GET: ProductCategory
        public ActionResult Index(string searchName, int ?page, string currentFilter, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            // phan trang
            if (searchName != null)
            {
                page = 1;
            }
            else
            {
                searchName = currentFilter;
            }
            ViewBag.CurrentFilter = searchName;
            IQueryable<CategoryProduct> lstCP = DataProvider.Entities.CategoryProducts;
            if (!string.IsNullOrEmpty(searchName))
            {
                lstCP = lstCP.Where(p => p.Name.Contains(searchName));
            }
            
            //sort
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSort = string.IsNullOrEmpty(sortOrder) ? "nameSort" : "";
            switch (sortOrder)
            {
                case "nameSort":
                    lstCP = lstCP.OrderBy(p => p.Name);
                    break;
                default:
                    lstCP = lstCP.OrderBy(p => p.DisplayOrder);
                    break;
            }
            //
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstCP.ToPagedList(pageNumber,pageSize));
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryProduct objCP)
        {
            if (objCP != null)
            {
                objCP.CreateDate = DateTime.Now.Date;
                DataProvider.Entities.CategoryProducts.Add(objCP);
                DataProvider.Entities.SaveChanges();
            }
            return RedirectToAction("Index", "ProductCategory", objCP);
        }
        public ActionResult Edit(int id)
        {
            var objCP = DataProvider.Entities.CategoryProducts.Find(id);
            
            if (objCP != null) return View(objCP);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryProduct objCP)
        {
            var objCPOlder = DataProvider.Entities.CategoryProducts.Find(id);
            if (objCPOlder != null)
            {
                objCP.ModifileDate = DateTime.Now.Date;
                objCP.ModifileBy = "Huyratio";
                DataProvider.Entities.Entry(objCPOlder).CurrentValues.SetValues(objCP);
                DataProvider.Entities.SaveChanges();
            }
            return RedirectToAction("Index", "ProductCategory");
        }
        public ActionResult Details(int id)
        {
            var objCP = DataProvider.Entities.CategoryProducts.Find(id);
            if (objCP != null) return View(objCP);
            return View();
        }
        public ActionResult Delete(int id)
        {
            var objCP = DataProvider.Entities.CategoryProducts.Find(id);
            if (objCP != null)
            {
                DataProvider.Entities.CategoryProducts.Remove(objCP);
                DataProvider.Entities.SaveChanges();                
            }
            return RedirectToAction("Index", "ProductCategory");
        }
    }
}