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
    [Role("Admin", "User")]
    public class PhanCongController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: PhanCong
        public ActionResult Index()
        {
            var phanCong = db.PhanCong.Include(p => p.NguoiDung);
            return View(phanCong.OrderByDescending(x => x.NgayTao).ToList());
        }

        // GET: PhanCong/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanCong phanCong = db.PhanCong.Find(id);
            if (phanCong == null)
            {
                return HttpNotFound();
            }
            return View(phanCong);
        }

        // GET: PhanCong/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhanCong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,HocKi,NgayTao")] PhanCong phanCong)
        {
            if (ModelState.IsValid)
            {
                phanCong.NguoiDung_ID = NguoiDungLib.Get().ID;
                db.PhanCong.Add(phanCong);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = phanCong.ID });
            }

            return View(phanCong);
        }

        // GET: PhanCong/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanCong phanCong = db.PhanCong.Find(id);
            if (phanCong == null)
            {
                return HttpNotFound();
            }
            return View(phanCong);
        }

        // POST: PhanCong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NguoiDung_ID,HocKi,NgayTao")] PhanCong phanCong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phanCong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phanCong);
        }

        // GET: PhanCong/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanCong phanCong = db.PhanCong.Find(id);
            if (phanCong == null)
            {
                return HttpNotFound();
            }
            return View(phanCong);
        }

        // POST: PhanCong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PhanCong phanCong = db.PhanCong.Find(id);
            db.PhanCong.Remove(phanCong);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: PhanCong/Create
        public ActionResult ThemNhiemVu(long? phancong_id)
        {
            if (phancong_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanCong phanCong = db.PhanCong.Find(phancong_id);
            if (phanCong == null)
            {
                return HttpNotFound();
            }
            ThemNhiemVuModel themNhiemVuModel = new ThemNhiemVuModel();
            themNhiemVuModel.PhanCong_ID = phanCong.ID;
            return View(themNhiemVuModel);
        }

        // POST: PhanCong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemNhiemVu([Bind(Include = "ID,PhanCong_ID,TenGV,MaHP,TenLop,LoaiPhong,NhomLT,NhomHT,GhiChu")] ThemNhiemVuModel themNhiemVuModel)
        {
            if (ModelState.IsValid)
            {
                NhiemVu nhiemVu = new NhiemVu();
                nhiemVu.PhanCong_ID = themNhiemVuModel.PhanCong_ID;
                GiangVien giangVien = db.GiangVien.Where(x => x.TenGV == themNhiemVuModel.TenGV).SingleOrDefault();
                if (giangVien == null)
                {
                    ModelState.AddModelError("TenGV", "Tên giảng viên không tồn tại trên hệ thống!");
                    return View(themNhiemVuModel);
                }
                nhiemVu.GiangVien_ID = giangVien.ID;
                HocPhan hocPhan = db.HocPhan.Where(x => x.MaHP == themNhiemVuModel.MaHP).SingleOrDefault();
                if (hocPhan == null)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần không tồn tại trên hệ thống!");
                    return View(themNhiemVuModel);
                }
                if (!PhanCongLib.IsHocPhanOfGiangVien(giangVien.TenGV, hocPhan.MaHP))
                {
                    ModelState.AddModelError("MaHP", "Mã học phần này giảng viên \"" + giangVien.TenGV + "\" không có dạy");
                    return View(themNhiemVuModel);
                }
                nhiemVu.HocPhan_ID = hocPhan.ID;
                Lop lop = db.Lop.Where(x => x.TenLop == themNhiemVuModel.TenLop).SingleOrDefault();
                if (lop == null)
                {
                    ModelState.AddModelError("TenLop", "Tên lớp không tồn tại trên hệ thống!");
                    return View(themNhiemVuModel);
                }
                if (!PhanCongLib.IsLopOfHocPhan(hocPhan.MaHP, lop.TenLop))
                {
                    ModelState.AddModelError("TenLop", "Tên lớp này không có dạy học phần \"" + hocPhan.MaHP + "\"");
                    return View(themNhiemVuModel);
                }
                nhiemVu.Lop_ID = lop.ID;
                nhiemVu.LoaiPhong = themNhiemVuModel.LoaiPhong;
                nhiemVu.NhomLT = themNhiemVuModel.NhomLT;
                nhiemVu.NhomHT = themNhiemVuModel.NhomHT;
                nhiemVu.GhiChu = themNhiemVuModel.GhiChu;
                db.NhiemVu.Add(nhiemVu);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = themNhiemVuModel.PhanCong_ID });
            }

            return View(themNhiemVuModel);
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
