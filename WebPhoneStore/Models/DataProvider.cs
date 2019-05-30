using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPhoneStore.Models
{
    public class DataProvider
    {
        private static WebBanDienThoaiEntities _Entities = new WebBanDienThoaiEntities();
        public static WebBanDienThoaiEntities Entities
        {
            get { return _Entities; }
            set { _Entities = value; }
        }
    }
}