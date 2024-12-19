using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Areas.Admin.Controllers
{
    public class HangSanXuatController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var list = db.HangSanXuats.ToList();
            return View(list);
        }
        public ActionResult ThemHangSanXuat()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            return View();
        }
        [HttpPost]
        public ActionResult postThemHangSanXuat(FormCollection form)
        {
            var tenHSX = form["tenhangsanxuat"];
            var thongTin = form["thongtin"];
            HangSanXuat hsx = new HangSanXuat();
            hsx.TenHangSanXuat = tenHSX;
            hsx.ThongTin = thongTin;
            db.HangSanXuats.InsertOnSubmit(hsx);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ChinhSuaHangSanXuat(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var hsx = db.HangSanXuats.Where(o => o.IdHangSanXuat == id);
            if (hsx == null){
                return HttpNotFound("Không tìm thấy hãng sản xuất!");
            }
            return View(hsx);
        }
        [HttpPost]
        public ActionResult ChinhSuaHangSanXuat(FormCollection form)
        {
            var idHSX = form["idHSX"];
            var tenHSX = form["tenhangsanxuat"];
            var thongTin = form["thongtin"];
            if (int.TryParse(idHSX, out int id))
            {
                // Tìm khách hàng bằng Id
                var hangSanXuat = db.HangSanXuats.FirstOrDefault(o => o.IdHangSanXuat == id);

                if (hangSanXuat != null)
                {
                    hangSanXuat.TenHangSanXuat = tenHSX;
                    hangSanXuat.ThongTin = thongTin;
                    db.SubmitChanges();
                    ViewBag.SuccessMessage = "Cập nhật thành công!";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ErrorMessage = "Cập nhật thất bại!";
            return RedirectToAction("Index");
        }
    }
}