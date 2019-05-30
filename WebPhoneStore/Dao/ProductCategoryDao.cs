using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoneStore.Models;

namespace WebPhoneStore.Dao
{
    public class ProductCategoryDao
    {
        WebBanDienThoaiEntities db = null;
        public ProductCategoryDao()
        {
            db = new WebBanDienThoaiEntities();
        }
        public List<CategoryProduct> listAll()
        {
            return db.CategoryProducts.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }
        public List<Product> lstProduct()
        {
            return db.Products.OrderByDescending(x => x.CreateDate).ToList();
        }
       
        public List<CategoryProduct> topListCategory(int top)
        {
            return db.CategoryProducts.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).Take(top).ToList();
        }
        public CategoryProduct getByID(long ?ID)
        {
            return db.CategoryProducts.Find(ID);
        }
    }
}