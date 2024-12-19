using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THD_Mobile.Models
{
    public class GioHangViewModel
    {
        public int IdGioHang { get; set; }      // ID của giỏ hàng
        public int IdSanPham { get; set; }      // ID sản phẩm
        public string TenSanPham { get; set; }  // Tên sản phẩm
        public int SLTonKho { get; set; }
        public string Anh { get; set; }  // Tên sản phẩm
        public decimal Gia { get; set; }        // Giá của sản phẩm
        public int SoLuongSanPham { get; set; } // Số lượng sản phẩm trong giỏ hàng
        public decimal ThanhTien => Gia * SoLuongSanPham; // Thành tiền (tự động tính)
        public DateTime? NgayThem { get; set; }  // Ngày thêm vào giỏ hàng
    }
}