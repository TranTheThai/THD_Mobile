using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Areas.Customer.Controllers
{
    public class GioHangController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        // GET: GioHang
        public ActionResult IndexGioHang()
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

            var gioHangItems = (from gh in db.GioHangs
                                join sp in db.SanPhams on gh.IdSanPham equals sp.IdSanPham
                                where gh.IdKhachHang == idKhachHang
                                select new GioHangViewModel
                                {
                                    IdGioHang = gh.IdGioHang,
                                    IdSanPham = sp.IdSanPham,
                                    TenSanPham = sp.TenSanPham,
                                    SLTonKho = sp.SoLuong,
                                    Gia = sp.Gia,
                                    Anh = sp.Anh,
                                    SoLuongSanPham = gh.SoLuongSanPham,
                                    NgayThem = gh.NgayThem ?? DateTime.Now
                                }).ToList();

            // Truyền dữ liệu sang View
            return View(gioHangItems);
        }
        [HttpPost]
        public ActionResult ThemVaoGioHang(int IdSanPham, int SoLuongSanPham)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            int idKhachHang;
            if (Session["idKhachHang"] == null || !int.TryParse(Session["idKhachHang"].ToString(), out idKhachHang))
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var sanPham = db.SanPhams.FirstOrDefault(sp => sp.IdSanPham == IdSanPham);
            if (sanPham == null)
            {
                return HttpNotFound("Không tìm thấy sản phẩm.");
            }
            var gioHang = db.GioHangs.FirstOrDefault(gh => gh.IdKhachHang == idKhachHang && gh.IdSanPham == IdSanPham);
            if (gioHang != null)
            {
                gioHang.SoLuongSanPham += SoLuongSanPham;
            }
            else
            {
                GioHang newItem = new GioHang
                {
                    IdKhachHang = idKhachHang,
                    IdSanPham = IdSanPham,
                    SoLuongSanPham = SoLuongSanPham,
                    NgayThem = DateTime.Now
                };
                db.GioHangs.InsertOnSubmit(newItem);
            }
            db.SubmitChanges();
            string Path = "/Customer/SanPham/ChiTietSanPham/";
            string fullPath = Path + "?IdSP=" + sanPham.IdSanPham;
            return Redirect(fullPath);
        }
        public ActionResult XoaSanPhamGioHang(int idSanPham)
        {
            int idKhachHang;
            if (Session["idKhachHang"] == null || !int.TryParse(Session["idKhachHang"].ToString(), out idKhachHang))
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var gioHangItem = db.GioHangs.SingleOrDefault(gh => gh.IdSanPham == idSanPham && gh.IdKhachHang == idKhachHang);
            if (gioHangItem != null)
            {
                db.GioHangs.DeleteOnSubmit(gioHangItem);
                db.SubmitChanges();
            }
            return RedirectToAction("IndexGioHang");
        }
    }
}