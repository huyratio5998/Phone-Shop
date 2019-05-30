using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoneStore.Models;

namespace WebPhoneStore.Dao
{
    public class ClientAboutDao
    {
        WebBanDienThoaiEntities db = null;
        public ClientAboutDao()
        {
            db = new WebBanDienThoaiEntities();
        }
        public About getAbout()
        {
            return db.Abouts.Where(p => p.Status == true).OrderByDescending(p=>p.CreateDate).FirstOrDefault();
        }
        public List<Content> getListBlog()
        {
            return db.Contents.Where(p => p.Status == true).OrderByDescending(p => p.CreateDate).ToList();
        }
        public IQueryable<Content> getLstBlog()
        {
            return db.Contents.Where(p => p.Status == true).OrderByDescending(p => p.CreateDate);
        }
        public List<Content> getLstBlogTop(int top)
        {
            return db.Contents.Where(x => x.Status == true).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }
        public About getAboutByID(int ? Id)
        {
            return db.Abouts.Where(x => x.ID == Id).FirstOrDefault();
        }
        public Content getBlogByID(int ? Id)
        {
            return db.Contents.Where(x => x.ID == Id).FirstOrDefault();
        }
            
    }
}