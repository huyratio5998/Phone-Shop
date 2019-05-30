using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Dao;
using WebPhoneStore.Models;
namespace WebPhoneStore.Controllers
{
    public class ClientBlogController : Controller
    {
        // GET: ClientBlog
        public ActionResult ClientBlog(string search)
        {
            var db = new ClientAboutDao();
            ViewBag.lstBlog = db.getListBlog();
            var productcategorydao = new ProductCategoryDao();
            ViewBag.lstCategoryTop = productcategorydao.topListCategory(6);
            //IQueryable<About> listBlog = DataProvider.Entities.Abouts
            ViewBag.lstHotProduct = new ProductDao().listProductHot(4);
            ViewBag.lstTags = new ProductDao().lstTags(4);
            // tim kiem
            var lstSearch = DataProvider.Entities.Contents.Where(x => x.Name.Contains(search) || x.Description.Contains(search)).OrderByDescending(x => x.CreateDate).ToList();
            if(search!=null && lstSearch != null)
            {
                ViewBag.lstBlog = lstSearch;
            }
            return View();
        }
        public ActionResult ClientBlogDetail(int ? ID)
        {
            var db = new ClientAboutDao();
            ViewBag.lstBlog = db.getListBlog();
            var productcategorydao = new ProductCategoryDao();
            ViewBag.lstCategoryTop = productcategorydao.topListCategory(6);            
            ViewBag.lstHotProduct = new ProductDao().listProductHot(4);
            ViewBag.lstTags = new ProductDao().lstTags(4);
            ViewBag.objBlog = db.getBlogByID(ID);
            return View();
        }
        [HttpPost]
        public ActionResult ClientBlogDetail(Feedback infor, int ? ID)
        {
            if (ModelState.IsValid)
            {
                if (infor != null)
                {
                    infor.CreateDate = DateTime.Now.Date;
                    DataProvider.Entities.Feedbacks.Add(infor);
                    DataProvider.Entities.SaveChanges();
                    var db = new ClientAboutDao();
                    ViewBag.lstBlog = db.getListBlog();
                    var productcategorydao = new ProductCategoryDao();
                    ViewBag.lstCategoryTop = productcategorydao.topListCategory(6);
                    ViewBag.lstHotProduct = new ProductDao().listProductHot(4);
                    ViewBag.lstTags = new ProductDao().lstTags(4);
                    ViewBag.objBlog = db.getBlogByID(ID);
                    return View();
                }
            }
            return RedirectToAction("ClientBlog", "ClientBlog");
        }        
    }
}