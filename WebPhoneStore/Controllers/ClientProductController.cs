using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Dao;
using WebPhoneStore.Models;

namespace WebPhoneStore.Controllers
{
    public class ClientProductController : Controller
    {
        // GET: ClientProduct
        
        public ActionResult Index(long ? CID, string tukhoa, decimal ? gia)
        {                                  
            List<Product> listProduct = DataProvider.Entities.Products.ToList();            
            if (CID != null)
            {
                listProduct=listProduct.Where(p => p.CategoryID == CID).ToList();
                
            }
            if (!string.IsNullOrEmpty(tukhoa))
            {
                listProduct = listProduct.Where(p => p.Name.Contains(tukhoa)).ToList();
            }
            if (gia != null)
            {
                listProduct = listProduct.Where(p =>(Math.Abs((decimal)(p.Price.GetValueOrDefault(0)-gia))<=1000000)).ToList();
            }
            if(CID==null&& string.IsNullOrEmpty(tukhoa)&&gia==null)
            {
                listProduct = new ProductDao().lstNewProduct(12);
            }
            listProduct = listProduct.OrderByDescending(p => p.CreateDate).ToList();
            ViewBag.lstProduct = listProduct;
            ViewBag.lstProductCategory = new ProductCategoryDao().listAll();
            return View();
        }
       
        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var db = new ProductCategoryDao();
            var model = db.listAll();
            ViewBag.lstProduct = db.lstProduct();
                return PartialView(model);
        }
        public ActionResult Details(int ID)
        {            
            var db = new ProductDao();
            var product = db.getProductById(ID);
            var lstP = db.lstALL();
            int count = 0;
            List<Product> lstRP = new List<Product>();
            foreach(Product item in lstP)
            {
                if (item.Equals(product)) continue;
                decimal a, b;
                if (item.Price.Equals(null))  a = 0; 
                else a = (decimal)item.Price;
                if (product.Price.Equals(null)) b = 0;                                
                else b = (decimal)product.Price;                
                
                if (item.CategoryID == product.CategoryID || Math.Abs((decimal)(a-b))<=1000000)
                {
                    lstRP.Add(item);
                    count++;
                }
                if (count == 8) break;
            }
            ViewBag.lstRP = lstRP;
            ViewBag.lstPC = new ProductCategoryDao().getByID((product.CategoryID));
            return View(product);
        }
       
    }
}