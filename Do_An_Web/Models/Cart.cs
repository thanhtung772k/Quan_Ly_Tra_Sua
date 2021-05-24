using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Do_An_Web.Models
{
    [Serializable]
    public class Cart
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public int Thanhtien
        {
            get { return SoLuong * DonGia; }
        }
    }
}