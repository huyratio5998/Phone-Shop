using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebPhoneStore.Dao;

namespace WebPhoneStore.Controllers
{
    public class HomeClientController : Controller
    {
        // GET: HomeClient        
        public ActionResult Index()
        {            
            ViewBag.NewProduct = new ProductDao().lstNewProduct(8);
            ViewBag.lstSlides = new SlidesDao().lstSlides();
            ViewBag.lstBlog = new ClientAboutDao().getLstBlogTop(3);
            ViewBag.lstAccount = new UserDao().getLstUsersTop(5);
            return View();
        }
        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = new MenuDao().lstByGroupID(1);
            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult FooterClient()
        {
            var model = new FooterDao().getFooter();
            return PartialView(model);
        }
    }
}