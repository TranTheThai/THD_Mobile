using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THD_Mobile.Models
{
    public class ChiTietDonHangViewModel
    {
        public int IdSanPham { get; set; }
        public int SoLuongSanPham { get; set; }
        public SanPham SanPham { get; set; }
        public string AnhSP { get; set; }
    }

}