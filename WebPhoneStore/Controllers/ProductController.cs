using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebPhoneStore.Models;
using PagedList;
namespace WebPhoneStore.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(string sortOrder, string searchName, string currentFilter, int ?page,int ? categoryID)
        {            
            if (searchName != null)
            {
                page = 1;
            }
            else
            {
                searchName = currentFilter;
            }
            ViewBag.CurrentFilter = searchName;
            IQueryable<Product> lstProduct = DataProvider.Entities.Products;
            if (!string.IsNullOrEmpty(searchName))
            {
                lstProduct = lstProduct.Where(p => p.Name.Contains(searchName));
            }
            if(categoryID!=null)
            {
                lstProduct = lstProduct.Where(p => p.CategoryID==categoryID);
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentSelect = categoryID;
            // sort column
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "sortName" : "";
            switch (sortOrder)
            {
                case "sortName":
                    lstProduct= lstProduct.OrderBy(p => p.Name);
                    break;
                default:
                   lstProduct= lstProduct.OrderBy(p => p.ID);
                    break;
            }
            var lstPC = DataProvider.Entities.CategoryProducts.ToList();
            ViewBag.ProductCategory = new SelectList(lstPC, "ID", "Name");
            ViewBag.lstPC = lstPC;
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstProduct.ToPagedList(pageNumber,pageSize));
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            // xu ly upload file: 
            if(file!=null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                file.SaveAs(Server.MapPath("~/Content/Images/" + fileName));
                // luu anh vao database
                product.Image = fileName;
            }
            DataProvider.Entities.Products.Add(product);
            DataProvider.Entities.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
        public ActionResult Edit(int id)
        {
            var lstCP = DataProvider.Entities.CategoryProducts.ToList();
            ViewBag.CategoryProduct = new SelectList(lstCP, "ID", "Name");
            Product product = DataProvider.Entities.Products.Find(id);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product, HttpPostedFileBase file)
        {
            Product olderP = DataProvider.Entities.Products.Find(id);
            if (olderP != null)
            {
                if(file!=null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Content/Images/" + fileName));
                    product.Image = fileName;
                }
                else
                {
                    product.Image = olderP.Image;
                }
                DataProvider.Entities.Entry(olderP).CurrentValues.SetValues(product);
                DataProvider.Entities.SaveChanges();
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
        public ActionResult Details(int id)
        {
            Product product = DataProvider.Entities.Products.Find(id);
            if (product != null)
            {
                return View(product);
            }
            return View();
        }
        public ActionResult Delete(int id)
        {
            Product product = DataProvider.Entities.Products.Find(id);
            if (product != null)
            {
                DataProvider.Entities.Products.Remove(product);
                DataProvider.Entities.SaveChanges();                
            }
            return RedirectToAction("Index", "Product");
        }


    }
}