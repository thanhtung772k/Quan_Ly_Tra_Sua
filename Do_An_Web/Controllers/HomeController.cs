using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Do_An_Web.Models;
using System.IO;

namespace Do_An_Web.Controllers
{
    public class HomeController : Controller
    {
        DemoooEntities db = new DemoooEntities();
        public ActionResult Index(int maloaisp = 0)
        {
            if (maloaisp == 0)
            {
                var sp = db.SanPhams.Include(s => s.LoaiSanPham);
                return View(sp.ToList());
            }
            else
            {
                var sp = db.SanPhams.Include(s => s.LoaiSanPham).Where(x => x.MaLoaiSP == maloaisp);
                return View(sp.ToList());
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult GetData()
        {
            DemoooEntities db = new DemoooEntities();
            var query = db.CTHoaDons.Include("SanPham")
                .GroupBy(p => p.SanPham.TenSP)
                .Select(g => new { name = g.Key, count = g.Sum(w=> w.SoLuong) }).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
            /*
             select MaSP,sum(SoLuong) as'So luong' from CTHoaDon
             group by MaSP
             */
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}