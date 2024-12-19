using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Areas.Customer.Controllers
{
    public class SanPhamController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        // GET: SanPham
        public ActionResult ChiTietSanPham(int? IdSP)
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

            if (IdSP == null)
            {
                return HttpNotFound(); // Nếu IdSP là null, trả về lỗi 404
            }
            SanPham sanpham = db.SanPhams.FirstOrDefault(o => o.IdSanPham == IdSP);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }
        [HttpGet]
        public ActionResult TimKiemSanPham(String contentSearch)
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

            if (string.IsNullOrEmpty(contentSearch))
            {
                var list = db.SanPhams.ToList();
                return View(list);
            }
            else
            {
                var list = db.SanPhams
                             .Where(s => s.TenSanPham.ToLower().Contains(contentSearch.ToLower()))
                             .ToList();

                return View(list);
            }
        }
        public ActionResult MuaSanPham()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            return View();
        }
    }
}