using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        // GET: Admin/SanPham
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var products = (from sp in db.SanPhams
                            join hsx in db.HangSanXuats
                            on sp.IdHangSanXuat equals hsx.IdHangSanXuat
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
        public ActionResult ThemSanPham()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var listHSX = db.HangSanXuats.ToList();
            return View(listHSX);
        }
        [HttpPost]
        public ActionResult ThemSanPham(FormCollection form, HttpPostedFileBase anh)
        {
            String tensp = form["tensanpham"];
            String gia = form["giasanpham"];
            String soluong = form["soluong"];
            String idHsx = form["hangsanxuat"];
            SanPham sanPham = new SanPham();
            sanPham.TenSanPham = tensp;
            sanPham.Gia = Convert.ToDecimal(gia);
            sanPham.SoLuong = Convert.ToInt32(soluong);
            sanPham.IdHangSanXuat = Convert.ToInt32(idHsx);

            //Ảnh
            if (anh == null)
            {
                ViewBag.ErrorMessage = "Chưa chọn ảnh";
            }
            if (anh.ContentLength == 0)
            {
                ViewBag.ErrorMessage = "Ảnh không có nội dung";
            }
            // xác định đường dẫn lưu
            var fileName = Path.GetFileName(anh.FileName);
            var urlTuongDoi = "~/Libs/Assets/Img_Products/";
            var urlTuyetDoi = Path.Combine(Server.MapPath(urlTuongDoi), fileName);
            anh.SaveAs(urlTuyetDoi);
            sanPham.Anh = urlTuongDoi + fileName;


            db.SanPhams.InsertOnSubmit(sanPham);
            db.SubmitChanges();
            ViewBag.SuccessMessage = "Thêm Mới Thành Công!";
            return RedirectToAction("Index", "SanPham");
        }
        public ActionResult ChinhSuaSanPham(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var sanPham = db.SanPhams.SingleOrDefault(s => s.IdSanPham == id);

            if (sanPham == null)
            {
                return HttpNotFound();
            }
            var viewModel = new SanPhamViewModel
            {
                IdSanPham = sanPham.IdSanPham,
                TenSanPham = sanPham.TenSanPham,
                Gia = sanPham.Gia,
                SoLuong = sanPham.SoLuong,
                IdHangSanXuat = sanPham.IdHangSanXuat,
                Anh = sanPham.Anh
            };
            ViewBag.HangSanXuatList = new SelectList(db.HangSanXuats, "IdHangSanXuat", "TenHangSanXuat", sanPham.IdHangSanXuat);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSuaSanPham(SanPhamViewModel model, String idSP, HttpPostedFileBase anh)
        {
            if (ModelState.IsValid)
            {
                int IDSP = Convert.ToInt32(idSP);
                // Tìm sản phẩm cần cập nhật
                var sanPham = db.SanPhams.FirstOrDefault(s => s.IdSanPham == IDSP);

                if (sanPham == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin sản phẩm
                sanPham.TenSanPham = model.TenSanPham;
                sanPham.Gia = model.Gia;
                sanPham.SoLuong = model.SoLuong;
                sanPham.IdHangSanXuat = model.IdHangSanXuat;


                if (anh == null)
                {
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
                if (anh.ContentLength == 0)
                {
                    ViewBag.ErrorMessage = "Ảnh không có nội dung";
                    return RedirectToAction("ChinhSuaSanPham", "SanPham", new { id = IDSP });
                }
                // xác định đường dẫn lưu
                var fileName = Path.GetFileName(anh.FileName);
                var urlTuongDoi = "~/Libs/Assets/Img_Products/";
                var urlTuyetDoi = Path.Combine(Server.MapPath(urlTuongDoi), fileName);
                anh.SaveAs(urlTuyetDoi);
                sanPham.Anh = urlTuongDoi + fileName;

                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            // Nếu form có lỗi, trả lại View với các thông tin
            ViewBag.HangSanXuatList = new SelectList(db.HangSanXuats, "IdHangSanXuat", "TenHangSanXuat", model.IdHangSanXuat);
            return View(model);
        }
        public ActionResult XoaSanPham(int idSanPham)
        {
            var product = db.SanPhams.SingleOrDefault(gh => gh.IdSanPham == idSanPham);
            if (product != null)
            {
                db.SanPhams.DeleteOnSubmit(product);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}