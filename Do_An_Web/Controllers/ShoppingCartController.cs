using Do_An_Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An_Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private DemoooEntities db = new DemoooEntities();
        private string strCart = "Cart";
        // GET: ShoppingCart
        public ActionResult Index()
        {
            List<Cart> giohang = Session["giohang"] as List<Cart>;
            return View(giohang);
        }
        public RedirectToRouteResult AddToCart(int MaSP)
        {
            if (Session["giohang"] == null)
            {
                Session["giohang"] = new List<Cart>();
            }
            List<Cart> giohang = Session["giohang"] as List<Cart>;
            if (giohang.FirstOrDefault(m => m.MaSP == MaSP) == null)
            {
                SanPham sp = db.SanPhams.Find(MaSP);
                Cart newItem = new Cart();
                newItem.MaSP = MaSP;
                newItem.TenSP = sp.TenSP;
                newItem.SoLuong = 1;
                newItem.DonGia = Convert.ToInt32(sp.Gia);
                giohang.Add(newItem);
            }
            else
            {
                Cart cartItem = giohang.FirstOrDefault(m => m.MaSP == MaSP);
                cartItem.SoLuong++;
            }
            Session["giohang"] = giohang;
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Update(int MaSP, int txtSoluong)
        {
            List<Cart> giohang = Session["giohang"] as List<Cart>;
            Cart item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
            if (item != null)
            {
                item.SoLuong = txtSoluong;
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult DelCartItem(int MaSP)
        {
            List<Cart> giohang = Session["giohang"] as List<Cart>;
            Cart item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
            if (item != null)
            {
                giohang.Remove(item);
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }
        private Cart GetCart()
        {
            var cart = Session["cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                Session["cart"] = cart;
            }
            return cart;
        }
        public ActionResult Checkout()
        {

            return View("Checkout");

        }

        public ActionResult ProcessOrder()
        {
            ViewBag.MaPT = new SelectList(db.ThanhToans, "MaPT", "TenPT");
            return View();

        }
        private void SetViewBag(int? selec = null)
        {
            var dao = new Danhsach();
            ViewBag.PT = new SelectList(dao.ListAll(), "MaPT", "TenPT", selec);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessOrder([Bind(Include = "MaHD,NgayBan,MaKH,NoiNhan,MaPT,TongTien")] HoaDon t, FormCollection frc)
        {
            List<Cart> giohang = Session["giohang"] as List<Cart>;
            int tongtien = 0;
            foreach (Cart item in giohang)
            {
                tongtien += item.SoLuong * item.DonGia;
            }
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

            }
            ViewBag.MaPT = new SelectList(db.ThanhToans, "MaPT", "TenPT", t.MaPT);
            Session.Remove(strCart);
            return View("ProcessOrder");

        }
    }
}