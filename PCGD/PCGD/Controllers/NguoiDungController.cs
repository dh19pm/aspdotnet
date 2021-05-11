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
        public ActionResult Index()
        {
            return View(db.NguoiDung.ToList());
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
