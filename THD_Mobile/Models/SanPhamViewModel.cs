using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THD_Mobile.Models
{
    public class SanPhamViewModel
    {
        public int IdSanPham { get; set; }      // ID sản phẩm
        public int IdHangSanXuat { get; set; }
        public string TenHangSanXuat { get; set; }      // ID của giỏ hàng
        public string TenSanPham { get; set; }  // Tên sản phẩm
        public string Anh { get; set; }  // Tên sản phẩm
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }        // Giá của sản phẩm
    }
}