using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: Admin/KhachHang
            DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var listKH = db.KhachHangs.Where(o => o.isDeleted == false).ToList();
            return View(listKH);
        }
        public ActionResult CapNhatKhachHang(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            if (id == null)
            {
                return HttpNotFound(); // Nếu IdSP là null, trả về lỗi 404
            }
            KhachHang kh = db.KhachHangs.FirstOrDefault(o => o.IdKhachHang == id);
            if (kh == null)
            {
                return HttpNotFound();
            }
            return View(kh);
        }
        [HttpPost]
        public ActionResult CapNhatKhachHang(FormCollection form)
        {
            String idKH = form["id"];
            String hoten = form["hoten"];
            String tendangnhap = form["tendangnhap"];
            String email = form["email"];
            String diachi = form["diachi"];
            String sdt = form["sdt"];
            if (int.TryParse(idKH, out int id))
            {
                // Tìm khách hàng bằng Id
                var khachHang = db.KhachHangs.FirstOrDefault(o => o.IdKhachHang == id);

                if (khachHang != null)
                {
                    khachHang.HoTen = hoten;
                    khachHang.TenDangNhap = tendangnhap;
                    khachHang.Email = email;
                    khachHang.DiaChi = diachi;
                    khachHang.SDT = sdt;
                    db.SubmitChanges();
                    ViewBag.SuccessMessage = "Cập nhật thành công!";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ErrorMessage = "Cập nhật thất bại!";
            return RedirectToAction("Index");
        }
        public ActionResult XoaKhachHang(int idKhachHang)
        {
            var cus = db.KhachHangs.SingleOrDefault(gh => gh.IdKhachHang == idKhachHang);
            if (cus != null)
            {
                cus.isDeleted = true;
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}