using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THD_Mobile.Models
{
    public class DonHangViewModel
    {
        public int IdDonHang { get; set; }
        public DateTime NgayTao { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public string TrangThaiHoaDon { get; set; }
        public int IdKhachHang { get; set; }
        public decimal TongGia { get; set; }
        public List<ChiTietDonHangViewModel> ChiTietDonHangs { get; set; }
        public string HoTenKhachHang { get; set; }
        public string EmailKhachHang { get; set; }
        public string DiaChiKhachHang { get; set; }
        public string SDTKhachHang { get; set; }
    }

}