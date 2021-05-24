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
    [Authentication]
    public class NguoiDungController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: NguoiDung
        [Role("Admin")]
        public ActionResult Index(string text = "", int page = 1)
        {
            var data = db.NguoiDung;
            page = (page > 0 ? page : 1);
            int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PaginationLimit"]);
            int start = (int)(page - 1) * pageSize;
            int totalPage = data.Where(x => x.TaiKhoan.Contains(text) || text == "").Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            if (page <= 0 || (page > numSize && numSize > 0))
            {
                return HttpNotFound();
            }
            this.ViewBag.searchString = text;
            this.ViewBag.Page = page;
            this.ViewBag.Total = numSize;
            return View(data.OrderByDescending(x => x.ID).Skip(start).Where(x => x.TaiKhoan.Contains(text) || text == "").Take(pageSize).ToList());
        }

        // GET: NguoiDung/Create
        [Role("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: NguoiDung/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Role("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,QuyenHan,TaiKhoan,MatKhau,XacNhanMatKhau")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                if (db.NguoiDung.Where(x => x.TaiKhoan == nguoiDung.TaiKhoan).Count() > 0)
                {
                    ModelState.AddModelError("TaiKhoan", "Tài khoản đã tồn tại trên hệ thống!");
                    return View(nguoiDung);
                }
                nguoiDung.MatKhau = Sha1.Convert(nguoiDung.MatKhau);
                nguoiDung.XacNhanMatKhau = nguoiDung.MatKhau;
                nguoiDung.NgayTao = DateTime.Now;
                db.NguoiDung.Add(nguoiDung);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nguoiDung);
        }

        // GET: NguoiDung/Edit/5
        [Role("Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDung.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // POST: NguoiDung/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Role("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,QuyenHan,TaiKhoan,MatKhau,XacNhanMatKhau")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                if (db.NguoiDung.Where(x => x.TaiKhoan == nguoiDung.TaiKhoan && x.ID != nguoiDung.ID).Count() > 0)
                {
                    ModelState.AddModelError("TaiKhoan", "Tài khoản đã tồn tại trên hệ thống!");
                    return View(nguoiDung);
                }
                nguoiDung.MatKhau = Sha1.Convert(nguoiDung.MatKhau);
                nguoiDung.XacNhanMatKhau = nguoiDung.MatKhau;
                nguoiDung.NgayTao = DateTime.Now;
                db.Entry(nguoiDung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nguoiDung);
        }

        // GET: NguoiDung/Delete/5
        [Role("Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDung.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // POST: NguoiDung/Delete/5
        [Role("Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NguoiDung nguoiDung = db.NguoiDung.Find(id);
            db.NguoiDung.Remove(nguoiDung);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: NguoiDung/Edit/5
        [Role("Admin", "User")]
        public ActionResult DoiMatKhau()
        {
            NguoiDung nguoiDung = db.NguoiDung.Find(NguoiDungLib.Get().ID);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            DoiMatKhauModel doiMatKhauModel = new DoiMatKhauModel();
            return View(doiMatKhauModel);
        }

        // POST: NguoiDung/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Role("Admin", "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMatKhau([Bind(Include = "MatKhau,MatKhauMoi,XacNhanMatKhau")] DoiMatKhauModel doiMatKhauModel)
        {
            if (ModelState.IsValid)
            {
                NguoiDung nguoiDung = db.NguoiDung.Find(NguoiDungLib.Get().ID);
                if (nguoiDung == null)
                {
                    return HttpNotFound();
                }
                doiMatKhauModel.MatKhau = Sha1.Convert(doiMatKhauModel.MatKhau);
                if (nguoiDung.MatKhau != doiMatKhauModel.MatKhau)
                {
                    ModelState.AddModelError("MatKhau", "Mật khẩu cũ chính xác!");
                    return View(doiMatKhauModel);
                }
                doiMatKhauModel.MatKhauMoi = Sha1.Convert(doiMatKhauModel.MatKhauMoi);
                doiMatKhauModel.XacNhanMatKhau = doiMatKhauModel.MatKhauMoi;
                nguoiDung.MatKhau = doiMatKhauModel.MatKhauMoi;
                nguoiDung.XacNhanMatKhau = doiMatKhauModel.MatKhauMoi;
                db.Entry(nguoiDung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(doiMatKhauModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
