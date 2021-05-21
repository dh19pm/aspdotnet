using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PCGD.Models;
using PCGD.Libs;

namespace PCGD.Controllers
{
    public class HomeController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (!string.IsNullOrEmpty(NguoiDungLib.Get().TaiKhoan))
                return HttpNotFound();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "TaiKhoan,MatKhau")] DangNhapModel dangNhapModel)
        {
            if (ModelState.IsValid)
            {
                dangNhapModel.MatKhau = Sha1.Convert(dangNhapModel.MatKhau);
                NguoiDung nguoiDung = db.NguoiDung.Where(x => x.TaiKhoan == dangNhapModel.TaiKhoan && x.MatKhau == dangNhapModel.MatKhau).SingleOrDefault();
                if (nguoiDung == null)
                {
                    ModelState.AddModelError("ThongBaoLoi", "Sai tài khoản hoặc mật khẩu!");
                    return View(dangNhapModel);
                }
                nguoiDung.MatKhau = null;
                NguoiDungLib.Set(nguoiDung);
                if (Request.QueryString["targetUrl"] != null)
                    return Redirect(HttpUtility.UrlDecode(Request.QueryString["targetUrl"]));
                return RedirectToAction("Index");
            }

            return View(dangNhapModel);
        }

        public ActionResult Logout()
        {
            if (string.IsNullOrEmpty(NguoiDungLib.Get().TaiKhoan))
                return HttpNotFound();
            NguoiDungLib.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Unauthorized()
        {
            if (string.IsNullOrEmpty(NguoiDungLib.Get().TaiKhoan))
                return HttpNotFound();
            return View();
        }
    }
}