using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoneStore.Models;

namespace WebPhoneStore.Dao
{
    public class FooterDao
    {
        WebBanDienThoaiEntities db = null;
        public FooterDao()
        {
            db = new WebBanDienThoaiEntities();
        }
        public Footer getFooter()
        {
            return db.Footers.SingleOrDefault(p => p.Status == true);
        }
    }
}