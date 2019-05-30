using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoneStore.Models;

namespace WebPhoneStore.Dao
{
    public class SlidesDao
    {
        WebBanDienThoaiEntities db = null;
        public SlidesDao()
        {
            db = new WebBanDienThoaiEntities();
        }
        public List<Slide> lstSlides()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }        
    }
}