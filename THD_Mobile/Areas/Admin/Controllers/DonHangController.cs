using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        public ActionResult IndexDonHang()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var donHangs = db.DonHangs.ToList();
            return View(donHangs);
        }
        public ActionResult ChiTietDonHang(int idDonHang)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var chiTietDonHang = db.DonHangs
                            .Where(dh => dh.IdDonHang == idDonHang)
                            .Select(dh => new DonHangViewModel
                            {
                                IdDonHang = dh.IdDonHang,
                                NgayTao = dh.NgayTao,
                                PhuongThucThanhToan = dh.PhuongThucThanhToan,
                                TrangThaiHoaDon = dh.TrangThaiHoaDon,
                                IdKhachHang = dh.IdKhachHang,
                                ChiTietDonHangs = dh.ChiTietDonHangs
                                                .Select(ct => new ChiTietDonHangViewModel
                                                {
                                                    IdSanPham = ct.IdSanPham,
                                                    SoLuongSanPham = ct.SoLuongSanPham,
                                                    SanPham = ct.SanPham
                                                }).ToList(),
                                HoTenKhachHang = dh.KhachHang.HoTen,
                                EmailKhachHang = dh.KhachHang.Email,
                                DiaChiKhachHang = dh.KhachHang.DiaChi,
                                SDTKhachHang = dh.KhachHang.SDT
                            }).FirstOrDefault();

            // Kiểm tra nếu đơn hàng không tồn tại
            if (chiTietDonHang == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại!");
            }

            // Tính tổng giá trị đơn hàng từ các chi tiết đơn hàng
            chiTietDonHang.TongGia = chiTietDonHang.ChiTietDonHangs.Sum(ctdh => ctdh.SanPham.Gia * ctdh.SoLuongSanPham);

            // Truyền dữ liệu vào View
            return View(chiTietDonHang);
        }
        [HttpPost]
        public ActionResult CapNhatTrangThai(int IdDonHang, string TrangThaiHoaDon)
        {
            var donHang = db.DonHangs.FirstOrDefault(dh => dh.IdDonHang == IdDonHang);

            if (donHang != null)
            {
                // Cập nhật trạng thái đơn hàng
                donHang.TrangThaiHoaDon = TrangThaiHoaDon;
                db.SubmitChanges();
            }

            // Trả về kết quả JSON để AJAX có thể xử lý (ở đây chỉ cần thông báo thành công)
            return Json(new { success = true });
        }
        public ActionResult XoaDonHang(int id)
        {
            var chiTietDonHangs = db.ChiTietDonHangs.Where(ct => ct.IDDonHang == id).ToList();
            if (chiTietDonHangs.Any())
            {
                // Cộng lại số lượng cho các sản phẩm
                foreach (var chiTiet in chiTietDonHangs)
                {
                    var sanPham = db.SanPhams.SingleOrDefault(sp => sp.IdSanPham == chiTiet.IdSanPham);
                    if (sanPham != null)
                    {
                        sanPham.SoLuong += chiTiet.SoLuongSanPham;
                    }
                }
                // Xóa các chi tiết đơn hàng
                db.ChiTietDonHangs.DeleteAllOnSubmit(chiTietDonHangs);
            }
            // Xóa đơn hàng
            var donHang = db.DonHangs.SingleOrDefault(dh => dh.IdDonHang == id);
            if (donHang != null)
            {
                db.DonHangs.DeleteOnSubmit(donHang);
            }
            db.SubmitChanges();
            return RedirectToAction("IndexDonHang", "DonHang");
        }
    }
}