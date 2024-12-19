using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Controllers
{
    public class SanPhamController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        // GET: SanPham
        public ActionResult ChiTietSanPham(int? IdSP)
        {
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
            int count = db.SanPhams
              .Where(s => s.TenSanPham.ToLower().Contains(contentSearch.ToLower()))
              .Count();
            if (count == 0) {
                ViewBag.ErrorMessage = "Không tìm thấy sản phẩm!";
                var list = db.SanPhams
                             .Where(s => s.TenSanPham.ToLower().Contains(contentSearch.ToLower()))
                             .ToList();

                return View(list);
            }
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
    }
}
