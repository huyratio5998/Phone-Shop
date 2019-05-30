using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebPhoneStore.Common;
using WebPhoneStore.Models;

namespace WebPhoneStore.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //public ActionResult GetSession()
        //{
        //    if (Session[CommonConstants.USER_SESSION] == null)
        //    {
        //        HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //        if (authCookie == null)
        //            return null;
        //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
        //        var cookie = ticket.Name;
        //        if (string.IsNullOrEmpty(cookie))
        //        {
        //            return View("Login", "Authentication");
        //        }

        //        var user = DataProvider.Entities.Users.Where(t => t.UserName == cookie).FirstOrDefault();
        //        Session[CommonConstants.USER_SESSION] = user;
        //    }

        //    return null;
        //}
    }
}