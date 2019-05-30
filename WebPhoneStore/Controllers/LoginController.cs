using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Common;
using WebPhoneStore.Dao;
using WebPhoneStore.Models;
namespace WebPhoneStore.Controllers
{
    public class LoginController : Controller
    {
        private WebBanDienThoaiEntities db = new WebBanDienThoaiEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //User obj = db.Users.Where(p => p.UserName.Equals(objUser.UserName) && p.Password.Equals(objUser.Password)).FirstOrDefault();
                //if (obj != null)
                //{
                //    Session["UserID"] = obj.ID.ToString();
                //    Session["UserName"] = obj.UserName.ToString();
                //    return RedirectToAction("UserHome");
                //}
                UserDao dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result==1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                        return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if(result == -1){
                    ModelState.AddModelError("", "Tài khoản chưa được kích hoạt");
                }
                else if(result == -2){
                    ModelState.AddModelError("", "Sai password");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng");
                }
               
            }
            return View("Login");
        }
        public ActionResult UserHome()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}