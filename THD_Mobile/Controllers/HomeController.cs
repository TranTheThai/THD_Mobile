using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THD_Mobile.Models;

namespace THD_Mobile.Controllers
{
    public class HomeController : Controller
    {
        DataDataContext db = new DataDataContext("Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=THD_Mobile;Integrated Security=True;TrustServerCertificate=True");
        public ActionResult Index()
        {
            var list = db.SanPhams.ToList();
            return View(list);
        }

        public ActionResult SanPhamTheoHang(String idHang)
        {
            var list = db.SanPhams.Where(s => s.IdHangSanXuat == Convert.ToInt32(idHang)).ToList();
            return View(list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}