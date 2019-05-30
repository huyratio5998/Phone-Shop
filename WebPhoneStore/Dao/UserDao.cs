using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoneStore.Models;
namespace WebPhoneStore.Dao
{
    public class UserDao
    {
        WebBanDienThoaiEntities db = null;
        public UserDao()
        {
            db = new WebBanDienThoaiEntities();
        }
        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public int Login(string userName, string passWord)
        {
            var result = db.Users.SingleOrDefault(p => p.UserName == userName );
            if (result ==null)
            {
                return 0;
            }
            else
            {
                if (result.Status==false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == passWord) return 1;
                    else return -2;
                }
                
            }
        }
        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(p => p.UserName == userName);
        }
        public List<User> getLstUsersTop(int top)
        {
            return db.Users.Where(x => x.Status == true).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }
    }
}