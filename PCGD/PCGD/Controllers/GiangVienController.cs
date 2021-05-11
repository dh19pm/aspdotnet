﻿using System;
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
    [Role("Admin")]
    public class GiangVienController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: GiangVien
        public ActionResult Index()
        {
            return View(db.GiangVien.OrderByDescending(x => x.ID).ToList());
        }

        // GET: GiangVien/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangVien.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            ViewGiangVienModel viewGiangVienModel = new ViewGiangVienModel();
            viewGiangVienModel.GiangVien_ID = giangVien.ID;
            viewGiangVienModel.HocPhan = HocPhanLib.GetHocPhanGiangVienModels(giangVien.ID);
            return View(viewGiangVienModel);
        }

        // GET: GiangVien/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GiangVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenGV")] GiangVien giangVien)
        {
            if (ModelState.IsValid)
            {
                if (db.GiangVien.Where(x => x.TenGV == giangVien.TenGV).Count() > 0)
                {
                    ModelState.AddModelError("TenGV", "Tên giảng viên đã tồn tại trên hệ thống!");
                    return View(giangVien);
                }

                db.GiangVien.Add(giangVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(giangVien);
        }

        // GET: GiangVien/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangVien.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            return View(giangVien);
        }

        // POST: GiangVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenGV")] GiangVien giangVien)
        {
            if (ModelState.IsValid)
            {
                if (db.GiangVien.Where(x => x.TenGV == giangVien.TenGV && x.ID != giangVien.ID).Count() > 0)
                {
                    ModelState.AddModelError("TenGV", "Tên giảng viên đã tồn tại trên hệ thống!");
                    return View(giangVien);
                }

                db.Entry(giangVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(giangVien);
        }

        // GET: GiangVien/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangVien.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            return View(giangVien);
        }

        // POST: GiangVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            GiangVien giangVien = db.GiangVien.Find(id);
            db.GiangVien.Remove(giangVien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: GiangVien/Create
        public ActionResult ThemHocPhan(long? giangvien_id)
        {
            GiangVien giangVien = db.GiangVien.Find(giangvien_id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            ThemHocPhanGiangVienModel themHocPhanGiangVienModel = new ThemHocPhanGiangVienModel();
            themHocPhanGiangVienModel.GiangVien_ID = giangVien.ID;
            return View(themHocPhanGiangVienModel);
        }

        // POST: GiangVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemHocPhan([Bind(Include = "GiangVien_ID,MaHP")] ThemHocPhanGiangVienModel themHocPhanGiangVienModel)
        {
            if (ModelState.IsValid)
            {
                HocPhan hocPhan = db.HocPhan.Where(x => x.MaHP == themHocPhanGiangVienModel.MaHP).SingleOrDefault();
                if (hocPhan == null)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần không tồn tại trên hệ thống!");
                    return View(themHocPhanGiangVienModel);
                }
                if (db.ChiTietGiangVien.Where(x => x.GiangVien_ID == themHocPhanGiangVienModel.GiangVien_ID && x.HocPhan_ID == hocPhan.ID).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã tồn tại!");
                    return View(themHocPhanGiangVienModel);
                }
                ChiTietGiangVien chiTietGiangVien = new ChiTietGiangVien();
                chiTietGiangVien.GiangVien_ID = themHocPhanGiangVienModel.GiangVien_ID;
                chiTietGiangVien.HocPhan_ID = hocPhan.ID;
                db.ChiTietGiangVien.Add(chiTietGiangVien);
                db.SaveChanges();
                return RedirectToAction("Details", "GiangVien", new { id = themHocPhanGiangVienModel.GiangVien_ID });
            }

            return View(themHocPhanGiangVienModel);
        }

        // GET: GiangVien/Edit/5
        public ActionResult SuaHocPhan(long? chitietgiangvien_id)
        {
            if (chitietgiangvien_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietGiangVien chiTietGiangVien = db.ChiTietGiangVien.Find(chitietgiangvien_id);
            if (chiTietGiangVien == null)
            {
                return HttpNotFound();
            }
            HocPhan hocPhan = db.HocPhan.Where(x => x.ID == chiTietGiangVien.HocPhan_ID).SingleOrDefault();
            if (hocPhan == null)
            {
                return HttpNotFound();
            }
            SuaHocPhanGiangVienModel suaHocPhanGiangVienModel = new SuaHocPhanGiangVienModel();
            suaHocPhanGiangVienModel.ChiTietGiangVien_ID = chiTietGiangVien.ID;
            suaHocPhanGiangVienModel.MaHP = hocPhan.MaHP;
            suaHocPhanGiangVienModel.GiangVien_ID = chiTietGiangVien.GiangVien_ID;
            return View(suaHocPhanGiangVienModel);
        }

        // POST: GiangVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaHocPhan([Bind(Include = "ChiTietGiangVien_ID,GiangVien_ID,MaHP")] SuaHocPhanGiangVienModel suaHocPhanGiangVienModel)
        {
            if (ModelState.IsValid)
            {
                ChiTietGiangVien chiTietGiangVien = db.ChiTietGiangVien.Find(suaHocPhanGiangVienModel.ChiTietGiangVien_ID);
                if (chiTietGiangVien == null)
                {
                    return HttpNotFound();
                }
                HocPhan hocPhan = db.HocPhan.Where(x => x.MaHP == suaHocPhanGiangVienModel.MaHP).SingleOrDefault();
                if (hocPhan == null)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần không tồn tại trên hệ thống!");
                    return View(suaHocPhanGiangVienModel);
                }
                if (db.ChiTietGiangVien.Where(x => x.ID != chiTietGiangVien.ID && x.HocPhan_ID == hocPhan.ID).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã tồn tại!");
                    return View(suaHocPhanGiangVienModel);
                }
                chiTietGiangVien.HocPhan_ID = hocPhan.ID;
                db.Entry(chiTietGiangVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "GiangVien", new { id = chiTietGiangVien.GiangVien_ID });
            }
            return View(suaHocPhanGiangVienModel);
        }

        // GET: GiangVien/XoaHocPhan/5
        public ActionResult XoaHocPhan(long? chitietgiangvien_id)
        {
            if (chitietgiangvien_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietGiangVien chiTietGiangVien = db.ChiTietGiangVien.Find(chitietgiangvien_id);
            if (chiTietGiangVien == null)
            {
                return HttpNotFound();
            }
            HocPhan hocPhan = db.HocPhan.Where(x => x.ID == chiTietGiangVien.HocPhan_ID).SingleOrDefault();
            if (hocPhan == null)
            {
                return HttpNotFound();
            }
            XoaHocPhanGiangVienModel xoaHocPhanGiangVienModel = new XoaHocPhanGiangVienModel();
            xoaHocPhanGiangVienModel.ChiTietGiangVien_ID = chiTietGiangVien.ID;
            xoaHocPhanGiangVienModel.MaHP = hocPhan.MaHP;
            xoaHocPhanGiangVienModel.GiangVien_ID = chiTietGiangVien.GiangVien_ID;
            return View(xoaHocPhanGiangVienModel);
        }

        // POST: GiangVien/XoaHocPhan/5
        [HttpPost, ActionName("XoaHocPhan")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaHocPhanConfirmed(long? chitietgiangvien_id)
        {
            ChiTietGiangVien chiTietGiangVien = db.ChiTietGiangVien.Find(chitietgiangvien_id);
            db.ChiTietGiangVien.Remove(chiTietGiangVien);
            db.SaveChanges();
            return RedirectToAction("Details", "GiangVien", new { id = chiTietGiangVien.GiangVien_ID });
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
