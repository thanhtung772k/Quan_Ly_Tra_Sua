using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Do_An_Web.Models
{
    public class ViewModel
    {
        public KhachHang khachhang { get; set; }
        public CTHoaDon cthd { get; set; }
        public HoaDon hoadon { get; set; }
        public LoaiSanPham loaisp { get; set; }
        public ThanhToan thanhtoan { get; set; }
        public SanPham sanpham { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.##0}")]
        public double Thanhtien { get; set; }
    }
}