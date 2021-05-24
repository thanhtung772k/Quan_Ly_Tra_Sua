using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using Do_An_Web.Models;

namespace Do_An_WebNC.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            using (DemoooEntities db = new DemoooEntities())
            {
                var loaisp = db.LoaiSanPhams.ToList();
                Hashtable tenloaisp = new Hashtable();
                foreach (var item in loaisp)
                {
                    tenloaisp.Add(item.MaLoaiSP, item.TenLoai);
                }
                ViewBag.TenLoaiSP = tenloaisp;
                return PartialView("Index");
            }

        }

    }
}