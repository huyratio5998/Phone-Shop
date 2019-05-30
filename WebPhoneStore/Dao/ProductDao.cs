using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoneStore.Models;

namespace WebPhoneStore.Dao
{
    public class ProductDao
    {
        WebBanDienThoaiEntities db = null;
        public ProductDao()
        {
            db = new WebBanDienThoaiEntities();
        }
        public List<Product> lstALL()
        {
            return db.Products.OrderByDescending(x => x.CreateDate).ToList();
        }
        public List<Product> lstNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }
        public List<Product> listProductHot(int top)
        {
            return db.Products.Where(x => x.Status == true && x.TopHot != null&&x.TopHot>=DateTime.Now).OrderByDescending(x => x.TopHot).Take(top).ToList();
        }
        public List<Tag> lstTags(int top)
        {
            return db.Tags.Take(top).ToList();
        }   
        public Product getProductById(long ID)
        {
            return db.Products.Find(ID);
        }
    }
}