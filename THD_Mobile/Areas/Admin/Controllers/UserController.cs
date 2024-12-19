using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            return View();
        }
        public ActionResult SanPhamTheoHang(String idHang)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var products = (from sp in db.SanPhams
                            join hsx in db.HangSanXuats
                            on sp.IdHangSanXuat equals hsx.IdHangSanXuat
                            where hsx.IdHangSanXuat == Convert.ToInt32(idHang)
                            select new SanPhamViewModel
                            {
                                IdSanPham = sp.IdSanPham,
                                TenSanPham = sp.TenSanPham,
                                Gia = sp.Gia,
                                SoLuong = sp.SoLuong,
                                Anh = sp.Anh,
                                TenHangSanXuat = hsx.TenHangSanXuat
                            }).ToList();

            return View(products);
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