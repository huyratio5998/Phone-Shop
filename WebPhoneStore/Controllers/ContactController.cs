using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebPhoneStore.Models;
namespace WebPhoneStore.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index(string content,string currentFilter, int ? page)
        {
            if (content != null)
            {
                page = 1;
            }
            else
            {
                content = currentFilter;
            }            
            ViewBag.CurrentFilter = content;
            IQueryable<Contact> lstContact = DataProvider.Entities.Contacts;
            if (!string.IsNullOrEmpty(content))
            {
                lstContact = lstContact.Where(p => p.Content.Contains(content));
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lstContact.OrderBy(p=>p.ID).ToPagedList(pageNumber,pageSize));
        }
       

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact objContact)
        {
            if (objContact != null)
            {
                DataProvider.Entities.Contacts.Add(objContact);
                DataProvider.Entities.SaveChanges();
            }
            return RedirectToAction("Index", "Contact");
            
        }
        public ActionResult Details(int id)
        {
            var objContact = DataProvider.Entities.Contacts.Find(id);
            if (objContact != null)
            {
                return View(objContact);
            }
            return View();
        }
        public ActionResult Delete(int id)
        {
            var objContact = DataProvider.Entities.Contacts.Find(id);
            if (objContact != null)
            {
                DataProvider.Entities.Contacts.Remove(objContact);
                DataProvider.Entities.SaveChanges();
            }
            return RedirectToAction("Index", "Contact");
        }
    }
}