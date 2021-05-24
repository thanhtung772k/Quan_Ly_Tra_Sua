using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Do_An_Web.Models
{
    public class Danhsach
    {
        DemoooEntities db = null;
        public Danhsach()
        {
            db = new DemoooEntities();
        }
        public List<ThanhToan> ListAll()
        {
            return db.ThanhToans.ToList();
        }
    }
}