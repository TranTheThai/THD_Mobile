using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Areas.Customer.Controllers
{
    public class MuaHangController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult MuaHang()
        //{
        //    int idKhachHang;
        //    if (Session["idKhachHang"] == null || !int.TryParse(Session["idKhachHang"].ToString(), out idKhachHang))
        //    {
        //        return RedirectToAction("DangNhap", "Auth", new { Area = "" });
        //    }

        //    return RedirectToAction("XacNhanDonHang", new { id = donHang.IdDonHang });
        //}
        //public ActionResult MuaChiTietSanPham(int IdSanPham, int SoLuongSanPham)
        //{
        //    int idKhachHang;
        //    if (Session["idKhachHang"] == null || !int.TryParse(Session["idKhachHang"].ToString(), out idKhachHang))
        //    {
        //        return RedirectToAction("DangNhap", "Auth", new { Area = "" });
        //    }
        //    decimal tongTienDonHang = 0;
        //    DonHang donHang = new DonHang
        //    {
        //        IdKhachHang = idKhachHang,
        //        NgayTao = DateTime.Now,
        //        PhuongThucThanhToan = "Thanh toán khi nhận hàng",
        //        TongGia = 0,
        //        TrangThaiHoaDon = "Chờ Xác Nhận"
        //    };
        //    db.DonHangs.InsertOnSubmit(donHang);
        //    db.SubmitChanges();
        //    var sanPham = db.SanPhams.SingleOrDefault(sp => sp.IdSanPham == IdSanPham);
        //    if (sanPham != null)
        //    {
        //        int quantity = Convert.ToInt32(SoLuongSanPham);
        //        if (quantity <= sanPham.SoLuong)
        //        {
        //            ChiTietDonHang chiTiet = new ChiTietDonHang
        //            {
        //                IDDonHang = donHang.IdDonHang,
        //                IdSanPham = IdSanPham,
        //                SoLuongSanPham = quantity
        //            };

        //            db.ChiTietDonHangs.InsertOnSubmit(chiTiet);
        //            sanPham.SoLuong -= quantity;
        //            tongTienDonHang += sanPham.Gia * quantity;
        //        }
        //        else
        //        {
        //            // Sản phẩm không đủ số lượng
        //            ViewBag.ErrorMessage = "Sản phẩm " + sanPham.TenSanPham + " không đủ số lượng.";
        //            return RedirectToAction("ChiTietSanPham", "SanPham", new { IdSP = IdSanPham});
        //        }
        //    }
        //    donHang.TongGia = tongTienDonHang;
        //    return RedirectToAction("XacNhanDonHang", new { id = donHang.IdDonHang });
        //}
        [HttpPost]
        public ActionResult MuaChiTietSanPham(int IdSanPham, int SoLuongSanPham)
        {
            int idKhachHang;
            if (Session["idKhachHang"] == null || !int.TryParse(Session["idKhachHang"].ToString(), out idKhachHang))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            decimal tongTienDonHang = 0;
            DonHang donHang = new DonHang
            {
                IdKhachHang = idKhachHang,
                NgayTao = DateTime.Now,
                PhuongThucThanhToan = "Thanh toán khi nhận hàng",
                TongGia = 0,
                TrangThaiHoaDon = "Chờ Xác Nhận"
            };

            db.DonHangs.InsertOnSubmit(donHang);
            db.SubmitChanges();

            var sanPham = db.SanPhams.SingleOrDefault(sp => sp.IdSanPham == IdSanPham);
            if (sanPham != null)
            {
                int quantity = SoLuongSanPham;
                if (quantity <= sanPham.SoLuong)
                {
                    ChiTietDonHang chiTiet = new ChiTietDonHang
                    {
                        IDDonHang = donHang.IdDonHang,
                        IdSanPham = IdSanPham,
                        SoLuongSanPham = quantity
                    };

                    db.ChiTietDonHangs.InsertOnSubmit(chiTiet);
                    sanPham.SoLuong -= quantity;
                    tongTienDonHang += sanPham.Gia * quantity;

                    donHang.TongGia = tongTienDonHang;

                    db.SubmitChanges();

                    // Trả về kết quả thành công
                    return Json(new { success = true, redirectUrl = Url.Action("XacNhanDonHang", new { id = donHang.IdDonHang }) });
                }
                else
                {
                    // Nếu sản phẩm không đủ số lượng
                    return Json(new { success = false, message = "Sản phẩm không đủ số lượng!" });
                }
            }
            else
            {
                // Nếu không tìm thấy sản phẩm
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            }
        }


        [HttpPost]
        public ActionResult MuaNhieuSanPham(List<int> selectedItems, FormCollection form)
        {
            if (selectedItems == null || selectedItems.Count == 0)
            {
                ViewBag.ErrorMessage = "Oops, Vui lòng chọn sản phẩm cần mua!";
                return RedirectToAction("IndexGioHang", "GioHang");
            }

            int idKhachHang;
            if (Session["idKhachHang"] == null || !int.TryParse(Session["idKhachHang"].ToString(), out idKhachHang))
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            decimal tongTienDonHang = 0;
            DonHang donHang = new DonHang
            {
                IdKhachHang = idKhachHang,
                NgayTao = DateTime.Now,
                PhuongThucThanhToan = "Thanh toán khi nhận hàng",
                TongGia = 0,
                TrangThaiHoaDon = "Chờ Xác Nhận"
            };
            db.DonHangs.InsertOnSubmit(donHang);
            db.SubmitChanges();
            foreach (var productId in selectedItems)
            {
                var sanPham = db.SanPhams.SingleOrDefault(sp => sp.IdSanPham == productId);
                if (sanPham != null)
                {
                    int quantity = Convert.ToInt32(form["quantity_" + productId]);
                    if (quantity <= sanPham.SoLuong)
                    {
                        ChiTietDonHang chiTiet = new ChiTietDonHang
                        {
                            IDDonHang = donHang.IdDonHang,
                            IdSanPham = productId,
                            SoLuongSanPham = quantity
                        };

                        db.ChiTietDonHangs.InsertOnSubmit(chiTiet);
                        sanPham.SoLuong -= quantity;
                        tongTienDonHang += sanPham.Gia * quantity;
                    }
                    else
                    {
                        // Sản phẩm không đủ số lượng
                        ViewBag.ErrorMessage = "Sản phẩm " + sanPham.TenSanPham + " không đủ số lượng.";
                        return RedirectToAction("IndexGioHang", "GioHang");
                    }
                }
            }

            // Cập nhật tổng giá cho đơn hàng
            donHang.TongGia = tongTienDonHang;

            // Lưu thay đổi
            db.SubmitChanges();

            // Xóa các sản phẩm đã mua khỏi giỏ hàng
            var gioHang = db.GioHangs.Where(g => selectedItems.Contains(g.IdSanPham) && g.IdKhachHang == idKhachHang).ToList();
            db.GioHangs.DeleteAllOnSubmit(gioHang);
            db.SubmitChanges();

            return RedirectToAction("XacNhanDonHang", new { id = donHang.IdDonHang });
        }
        public ActionResult XacNhanDonHang(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { Area = "" });
            }
            var donHang = db.DonHangs
                            .Where(dh => dh.IdDonHang == id)
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
                                                    SanPham = ct.SanPham,
                                                    AnhSP = ct.SanPham.Anh
                                                }).ToList(),
                                HoTenKhachHang = dh.KhachHang.HoTen,
                                EmailKhachHang = dh.KhachHang.Email,
                                DiaChiKhachHang = dh.KhachHang.DiaChi,
                                SDTKhachHang = dh.KhachHang.SDT
                            }).FirstOrDefault();

            if (donHang == null)
            {
                return HttpNotFound();
            }
            // Tính tổng giá trị đơn hàng từ các chi tiết đơn hàng
            donHang.TongGia = donHang.ChiTietDonHangs.Sum(ctdh => ctdh.SanPham.Gia * ctdh.SoLuongSanPham);

            return View(donHang);
        }
        public ActionResult HuyDonHang(int id)
        {
            var chiTietDonHangs = db.ChiTietDonHangs.Where(ct => ct.IDDonHang == id).ToList();
            if (chiTietDonHangs.Any())
            {
                // Cộng lại số lượng cho các sản phẩm
                foreach (var chiTiet in chiTietDonHangs)
                {
                    var sanPham = db.SanPhams.SingleOrDefault(sp => sp.IdSanPham == chiTiet.IdSanPham);
                    if (sanPham != null)
                    {
                        sanPham.SoLuong += chiTiet.SoLuongSanPham;
                    }
                }
                // Xóa các chi tiết đơn hàng
                db.ChiTietDonHangs.DeleteAllOnSubmit(chiTietDonHangs);
            }
            // Xóa đơn hàng
            var donHang = db.DonHangs.SingleOrDefault(dh => dh.IdDonHang == id);
            if (donHang != null)
            {
                db.DonHangs.DeleteOnSubmit(donHang);
            }
            db.SubmitChanges();
            return RedirectToAction("IndexGioHang", "GioHang");
        }
    }
}