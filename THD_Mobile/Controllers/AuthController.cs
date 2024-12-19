using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        // Xử lý đăng nhập
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult postDangNhap(FormCollection form)
        {
            String username = form["username"];
            String password = form["matkhau"];
            Admin admin = db.Admins.Where(o => o.TaiKhoan == username && o.MatKhau == password).FirstOrDefault();
            KhachHang khachHang = db.KhachHangs.Where(o => o.TenDangNhap == username && o.MatKhau == password).FirstOrDefault();
            if (admin != null)
            {
                ViewBag.SuccessMessage = "Đăng Nhập Thành Công!";
                Session["username"] = admin.TaiKhoan;
                Session["TenAdmin"] = admin.Ten;
                Session["password"] = admin.MatKhau;
                return RedirectToAction("Index", "SanPham", new { Area = "Admin" });
            }
            if (khachHang != null && khachHang.isDeleted == false)
            {
                ViewBag.SuccessMessage = "Đăng Nhập Thành Công!";
                Session["username"] = khachHang.TenDangNhap;
                Session["TenKhachHang"] = khachHang.HoTen;
                Session["idKhachHang"] = khachHang.IdKhachHang;
                Session["password"] = khachHang.MatKhau;
                return RedirectToAction("Index", "User", new { Area = "Customer" });
            }
            ViewBag.ErrorMessage = "Sai tài khoản hoặc mật khẩu!";
            return View("DangNhap");
        }
        // Xử lý đăng ký
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult postDangKy(FormCollection form)
        {
            String hoten = form["hoten"];
            String tendangnhap = form["tendangnhap"];
            String email = form["email"];
            String password = form["matkhau"];
            String confirm_password = form["conf-matkhau"];
                if(password == confirm_password)
            {
                KhachHang kh = new KhachHang();
                kh.HoTen = hoten;
                kh.TenDangNhap = tendangnhap;
                kh.Email = email;
                kh.MatKhau = password;
                kh.isDeleted = false;
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                ViewBag.SuccessMessage = "Đăng Ký Thành Công!";
                return RedirectToAction("DangNhap", "Auth");
            }
                
            ViewBag.ErrorMessage = "Đăng Ký Thất Bại!";
            return View("DangKy");
        }
    }
}