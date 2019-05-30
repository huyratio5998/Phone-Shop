using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebPhoneStore.Dao;
using WebPhoneStore.Models;

namespace WebPhoneStore.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private const string CartSession = "CartSession";
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new { status = true });
        }
        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(p => p.Product.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new { status = true });
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];
            //Decimal totalPrice=0;
            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
                    //totalPrice += (item.Product.Price.GetValueOrDefault(0) * item.Product.Quantity);
                
                
            }
            return Json(new { status = true });
        }
        public ActionResult Payment()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult Payment(string Name, string Phone, string Email, string Address)
        {
            var order = new Order();
            order.CreateDate = DateTime.Now;            
            order.ShipName = Name;
            order.ShipMobile = Phone;
            order.ShipEmail = Email;
            order.ShipAddress = Address;
            DataProvider.Entities.Orders.Add(order);
            DataProvider.Entities.SaveChanges();
            var cart = (List<CartItem>)Session[CartSession];
            try
            {
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = order.ID;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Price = item.Product.Price;
                    DataProvider.Entities.OrderDetails.Add(orderDetail);
                }
                DataProvider.Entities.Orders.Find(order.ID).Status = 1;
                DataProvider.Entities.SaveChanges();
            }
            catch(Exception e)
            {                                
                DataProvider.Entities.Orders.Find(order.ID).Status = -1;                
                DataProvider.Entities.SaveChanges();
            }                       
            return RedirectToAction("Index","Cart");
        }
        public ActionResult AddItem(long productID, int quantity)
        {
            var product = new ProductDao().getProductById(productID);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(p => p.Product.ID==product.ID))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == product.ID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[CartSession] = list;
                
            }
            else
            {
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                // gán vào session
                Session[CartSession] = list;
            }
            
            return Redirect(Request.UrlReferrer.ToString());
        }
    }

}