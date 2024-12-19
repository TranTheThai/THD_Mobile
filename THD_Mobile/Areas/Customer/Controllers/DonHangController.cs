using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Areas.Customer.Controllers
{
    public class DonHangController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        public ActionResult IndexDonHang()
        {
            
            int idKhachHang;
            if (Session["idKhachHang"] == null || !int.TryParse(Session["idKhachHang"].ToString(), out idKhachHang))
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }

            int soLuongSanPhamTrongGio = db.GioHangs
            .Where(gh => gh.IdKhachHang == idKhachHang)
            .Select(gh => gh.IdSanPham)
            .Distinct()
            .Count();
            ViewBag.SoLuongSanPhamTrongGio = soLuongSanPhamTrongGio;

            var donHangs = db.DonHangs.Where(o => o.IdKhachHang == idKhachHang).ToList();
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
        public ActionResult HuyDonHang(int id)
        {
            var donHang = db.DonHangs.FirstOrDefault(dh => dh.IdDonHang == id);

            if (donHang != null)
            {
                // Cập nhật trạng thái đơn hàng
                donHang.TrangThaiHoaDon = "Đã Huỷ Đơn";
                db.SubmitChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult BoHuyDonHang(int id)
        {
            var donHang = db.DonHangs.FirstOrDefault(dh => dh.IdDonHang == id);

            if (donHang != null)
            {
                // Cập nhật trạng thái đơn hàng
                donHang.TrangThaiHoaDon = "Chờ Xác Nhận";
                db.SubmitChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}