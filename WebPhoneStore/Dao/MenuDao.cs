using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoneStore.Models;

namespace WebPhoneStore.Dao
{

    public class MenuDao
    {
        WebBanDienThoaiEntities db = null;
        public MenuDao()
        {
            db = new WebBanDienThoaiEntities();
        }
        public List<Menu> lstByGroupID(int ID)
        {
            return db.Menus.Where(x => x.TypeID == ID).ToList();
        }

    }
}