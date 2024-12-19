using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Areas.Customer.Controllers
{
    public class UserController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        public ActionResult Index()
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

            var list = db.SanPhams.ToList();
            return View(list);
        }

        public ActionResult SanPhamTheoHang(String idHang)
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

            var list = db.SanPhams.Where(s => s.IdHangSanXuat == Convert.ToInt32(idHang)).ToList();
            return View(list);
        }
        public ActionResult Contact()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult DangXuat()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}