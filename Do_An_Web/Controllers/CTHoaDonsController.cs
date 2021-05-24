using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Do_An_Web.Models;

namespace Do_An_Web.Controllers
{
    public class CTHoaDonsController : Controller
    {
        private DemoooEntities db = new DemoooEntities();

        // GET: CTHoaDons
        public ActionResult Index(int MaHD)
        {
            using (DemoooEntities db = new DemoooEntities())
            {
                List<KhachHang> khachhang = db.KhachHangs.ToList();
                List<HoaDon> hoadon = db.HoaDons.ToList();
                List<ThanhToan> thanhtoan = db.ThanhToans.ToList();
                List<SanPham> sanpham = db.SanPhams.ToList();
                List<CTHoaDon> cthd = db.CTHoaDons.ToList();
                var main = from h in hoadon
                           join k in khachhang on h.MaKH equals k.MaKH
                           where (h.MaHD == MaHD)
                           select new ViewModel
                           {
                               hoadon = h,
                               khachhang = k
                           };
                var sub = from c in cthd
                          join s in sanpham on c.MaSP equals s.MaSP
                          where (c.MaHD == MaHD)
                          select new ViewModel
                          {
                              cthd = c,
                              sanpham = s,
                              Thanhtien = Convert.ToInt32(c.Gia * c.SoLuong)
                          };
                ViewBag.Main = main;
                ViewBag.Sub = sub;
                return View();
            }
        }

        // GET: CTHoaDons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHoaDon cTHoaDon = db.CTHoaDons.Find(id);
            if (cTHoaDon == null)
            {
                return HttpNotFound();
            }
            return View(cTHoaDon);
        }

        // GET: CTHoaDons/Create
        public ActionResult Create()
        {
            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "NoiNhan");
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP");
            return View();
        }

        // POST: CTHoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHD,MaSP,SoLuong,Gia")] CTHoaDon cTHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.CTHoaDons.Add(cTHoaDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "NoiNhan", cTHoaDon.MaHD);
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", cTHoaDon.MaSP);
            return View(cTHoaDon);
        }

        // GET: CTHoaDons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHoaDon cTHoaDon = db.CTHoaDons.Find(id);
            if (cTHoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "NoiNhan", cTHoaDon.MaHD);
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", cTHoaDon.MaSP);
            return View(cTHoaDon);
        }

        // POST: CTHoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHD,MaSP,SoLuong,Gia")] CTHoaDon cTHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTHoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "NoiNhan", cTHoaDon.MaHD);
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", cTHoaDon.MaSP);
            return View(cTHoaDon);
        }

        // GET: CTHoaDons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHoaDon cTHoaDon = db.CTHoaDons.Find(id);
            if (cTHoaDon == null)
            {
                return HttpNotFound();
            }
            return View(cTHoaDon);
        }

        // POST: CTHoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CTHoaDon cTHoaDon = db.CTHoaDons.Find(id);
            db.CTHoaDons.Remove(cTHoaDon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
