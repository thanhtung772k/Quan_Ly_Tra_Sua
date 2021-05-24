using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Do_An_Web.Models
{
    public class PhuongThuc
    {
        public List<SelectListItem> ThanhToan { set; get; }
        public int SelectedCityId { set; get; }
    }
}