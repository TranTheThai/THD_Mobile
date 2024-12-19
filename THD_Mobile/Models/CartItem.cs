using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THD_Mobile.Models
{
    public class CartItem
    {
        public int IdSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}