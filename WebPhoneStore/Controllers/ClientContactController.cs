using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Models;
namespace WebPhoneStore.Controllers
{
    public class ClientContactController : Controller
    {
        // GET: ClientContact
        public ActionResult Index(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                if (feedback != null)
                {
                    feedback.CreateDate = DateTime.Now.Date;    
                    DataProvider.Entities.Feedbacks.Add(feedback);
                    DataProvider.Entities.SaveChanges();                    
                }
            }
            return View();
        }
    }
}