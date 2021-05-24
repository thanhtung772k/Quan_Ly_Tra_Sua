using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Do_An_Web.Models;
using System.Net;
using System.Net.Mail;

namespace Do_An_Web.Controllers
{
    public class HoaDonsController : Controller
    {
        private DemoooEntities db = new DemoooEntities();

        // GET: HoaDons
        public ActionResult Index()
        {
            var hoaDons = db.HoaDons.Include(h => h.KhachHang).Include(h => h.ThanhToan);
            return View(hoaDons.ToList());
        }

        // GET: HoaDons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // GET: HoaDons/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH");
            ViewBag.MaPT = new SelectList(db.ThanhToans, "MaPT", "TenPT");
            return View();
        }

        // POST: HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHD,NgayBan,MaKH,NoiNhan,MaPT,TongTien")] HoaDon hoaDon, FormCollection frc, string Email, string Phone)
        {
            List<Cart> giohang = Session["giohang"] as List<Cart>;
            string sMsg = "<html><body><table border='1'><caption>Thông tin đặt hàng</caption><tr><th>STT</th><th>Tên Hàng</th><th>Số Lượng</th><th>Đơn Giá</th><th>Thành Tiền</th></tr>";
            var ma = hoaDon.MaPT;
            int i = 0;
            int tongtien = 0;
            foreach (Cart item in giohang)
            {
                i++;
                sMsg += "<tr>";
                sMsg += "<td>" + i.ToString() + "</td>";
                sMsg += "<td>" + item.TenSP + "</td>";
                sMsg += "<td>" + item.SoLuong.ToString() + "</td>";
                sMsg += "<td>" + item.DonGia.ToString() + "</td>";
                sMsg += "<td>" + String.Format("{0:#,###}", item.SoLuong*item.DonGia) + "</td>";
                sMsg += "</tr>";
                tongtien += item.SoLuong * item.DonGia;
            }
            sMsg += "<tr><th colspan='5'>Tổng cộng:&nbsp;"
                + String.Format("{0:#,###}", tongtien) + "&nbsp;VNĐ</th></tr><table>";
            MailMessage mail = new MailMessage("shinkute77@gmail.com", Email, "Thông tin đơn hàng", sMsg);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("shinkute77@gmail.com", "Anhyeuem1");
            mail.IsBodyHtml = true;
            //client.Send(mail);
            if (ModelState.IsValid)
            {
                KhachHang khach = new KhachHang()
                {
                    TenKH = frc["Name"],
                    DiaChi = frc["Address"],
                    SDT = frc["SDT"],
                    Email = frc["Email"],
                };
                db.KhachHangs.Add(khach);
                db.SaveChanges();
                HoaDon order = new HoaDon()
                {
                    MaKH = khach.MaKH,
                    NgayBan = DateTime.Now,
                    NoiNhan = frc["Address"],
                    MaPT = ma,
                    TongTien = tongtien
                };
                db.HoaDons.Add(order);
                db.SaveChanges();
                foreach (Cart cart in giohang)
                {
                    CTHoaDon chitiet = new CTHoaDon()
                    {
                        MaHD = order.MaHD,
                        MaSP = cart.MaSP,
                        SoLuong = cart.SoLuong,
                        Gia = cart.DonGia,
                    };
                    db.CTHoaDons.Add(chitiet);
                    db.SaveChanges();
                }
                giohang.Clear();
                frc.Clear();
                return RedirectToAction("ProcessOrder", "ShoppingCart");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", hoaDon.MaKH);
            ViewBag.MaPT = new SelectList(db.ThanhToans, "MaPT", "TenPT", hoaDon.MaPT);
            return View("ProcessOrder", "ShoppingCart");
        }

        // GET: HoaDons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", hoaDon.MaKH);
            ViewBag.MaPT = new SelectList(db.ThanhToans, "MaPT", "TenPT", hoaDon.MaPT);
            return View(hoaDon);
        }

        // POST: HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHD,NgayBan,MaKH,NoiNhan,MaPT,TongTien")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", hoaDon.MaKH);
            ViewBag.MaPT = new SelectList(db.ThanhToans, "MaPT", "TenPT", hoaDon.MaPT);
            return View(hoaDon);
        }

        // GET: HoaDons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // POST: HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HoaDon hoaDon = db.HoaDons.Find(id);
            db.HoaDons.Remove(hoaDon);
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
