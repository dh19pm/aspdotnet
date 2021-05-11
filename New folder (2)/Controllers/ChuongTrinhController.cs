using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PCGD.Models;

namespace PCGD.Controllers
{
    public class ChuongTrinhController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: ChuongTrinh
        public ActionResult Index()
        {
            return View(db.ChuongTrinh.ToList());
        }

        // GET: ChuongTrinh/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuongTrinh chuongTrinh = db.ChuongTrinh.Find(id);
            if (chuongTrinh == null)
            {
                return HttpNotFound();
            }
            List<HocPhanModel> hocPhanModel = (from c in db.ChuongTrinh
                join t in db.ChuongTrinh_HocPhan on c.ID equals t.ChuongTrinh_ID
                join h in db.HocPhan on t.HocPhan_ID equals h.ID
                where c.ID == chuongTrinh.ID
                orderby t.HocKi ascending, t.ID ascending
                select new HocPhanModel {
                ID = t.ID,
                HocPhan_ID = h.ID,
                MaHP = h.MaHP,
                TenHP = h.TenHP,
                LoaiHP = h.LoaiHP,
                HocKi = t.HocKi,
                SoTC  = h.SoTC,
                SoTietLT = t.SoTietLT,
                SoTietTH = t.SoTietTH
            }).ToList();
            ViewModel viewModel = new ViewModel();
            viewModel.ChuongTrinh = chuongTrinh;
            viewModel.HocPhan = hocPhanModel;
            return View(viewModel);
        }

        // GET: ChuongTrinh/ThemHocPhan/1
        public ActionResult ThemHocPhan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuongTrinh chuongTrinh = db.ChuongTrinh.Find(id);
            if (chuongTrinh == null)
            {
                return HttpNotFound();
            }
            ThemHocPhanModel themHocPhanModel = new ThemHocPhanModel();
            themHocPhanModel.ID = chuongTrinh.ID;
            return View(themHocPhanModel);
        }

        // POST: ChuongTrinh/ThemHocPhan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemHocPhan([Bind(Include = "ID,MaHP,HocKi,SoTietLT,SoTietTH")] ThemHocPhanModel themHocPhanModel)
        {
            if (ModelState.IsValid)
            {
                if (!themHocPhanModel.SoTietLT.HasValue && !themHocPhanModel.SoTietTH.HasValue)
                {
                    ModelState.AddModelError("ThongBaoLoi", "Lỗi! Vui lòng không bỏ trống cả trường số tiết lý thuyết và thực hành.");
                    return View(themHocPhanModel);
                }
                HocPhan hocPhan = db.HocPhan.Where(x => x.MaHP == themHocPhanModel.MaHP).FirstOrDefault();
                if (hocPhan == null)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần không tồn tại trên hệ thống!");
                    return View(themHocPhanModel);
                }
                if (db.ChuongTrinh_HocPhan.Where(x => x.ChuongTrinh_ID == themHocPhanModel.ID && x.HocPhan_ID == hocPhan.ID).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần này đã tồn tại trên chương trình!");
                    return View(themHocPhanModel);
                }
                ChuongTrinh_HocPhan chuongTrinh_HocPhan = new ChuongTrinh_HocPhan();
                chuongTrinh_HocPhan.ChuongTrinh_ID = themHocPhanModel.ID;
                chuongTrinh_HocPhan.HocPhan_ID = hocPhan.ID;
                chuongTrinh_HocPhan.HocKi = themHocPhanModel.HocKi;
                chuongTrinh_HocPhan.SoTietLT = themHocPhanModel.SoTietLT;
                chuongTrinh_HocPhan.SoTietTH = themHocPhanModel.SoTietTH;
                db.ChuongTrinh_HocPhan.Add(chuongTrinh_HocPhan);
                db.SaveChanges();
                return RedirectToAction("Details", "ChuongTrinh", new { id = themHocPhanModel.ID });
            }
            return View(themHocPhanModel);
        }

        // GET: ChuongTrinh/SuaHocPhan/5
        public ActionResult SuaHocPhan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuongTrinh_HocPhan chuongTrinh_HocPhan = db.ChuongTrinh_HocPhan.Find(id);
            if (chuongTrinh_HocPhan == null)
            {
                return HttpNotFound();
            }
            SuaHocPhanModel suaHocPhanModel = new SuaHocPhanModel();
            suaHocPhanModel.ID = chuongTrinh_HocPhan.ID;
            suaHocPhanModel.ChuongTrinh_ID = chuongTrinh_HocPhan.ChuongTrinh_ID;
            suaHocPhanModel.MaHP = db.HocPhan.Find(chuongTrinh_HocPhan.HocPhan_ID).MaHP;
            suaHocPhanModel.HocKi = chuongTrinh_HocPhan.HocKi;
            suaHocPhanModel.SoTietLT = chuongTrinh_HocPhan.SoTietLT;
            suaHocPhanModel.SoTietTH = chuongTrinh_HocPhan.SoTietTH;
            return View(suaHocPhanModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaHocPhan([Bind(Include = "ID,ChuongTrinh_ID,MaHP,HocKi,SoTietLT,SoTietTH")] SuaHocPhanModel suaHocPhanModel)
        {
            if (ModelState.IsValid)
            {
                if (!suaHocPhanModel.SoTietLT.HasValue && !suaHocPhanModel.SoTietTH.HasValue)
                {
                    ModelState.AddModelError("ThongBaoLoi", "Lỗi! Vui lòng không bỏ trống cả trường số tiết lý thuyết và thực hành.");
                    return View(suaHocPhanModel);
                }
                HocPhan hocPhan = db.HocPhan.Where(x => x.MaHP == suaHocPhanModel.MaHP).FirstOrDefault();
                if (hocPhan == null)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần không tồn tại trên hệ thống!");
                    return View(suaHocPhanModel);
                }
                ChuongTrinh_HocPhan chuongTrinh_HocPhan = db.ChuongTrinh_HocPhan.Find(suaHocPhanModel.ID);
                if (db.ChuongTrinh_HocPhan.Where(x => x.ChuongTrinh_ID == chuongTrinh_HocPhan.ChuongTrinh_ID && x.HocPhan_ID == hocPhan.ID && x.ID != chuongTrinh_HocPhan.ID).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần này đã tồn tại trên chương trình!");
                    return View(suaHocPhanModel);
                }
                chuongTrinh_HocPhan.HocPhan_ID = hocPhan.ID;
                chuongTrinh_HocPhan.HocKi = suaHocPhanModel.HocKi;
                chuongTrinh_HocPhan.SoTietLT = suaHocPhanModel.SoTietLT;
                chuongTrinh_HocPhan.SoTietTH = suaHocPhanModel.SoTietTH;
                db.Entry(chuongTrinh_HocPhan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ChuongTrinh", new { id = chuongTrinh_HocPhan.ChuongTrinh_ID });
            }
            return View(suaHocPhanModel);
        }

        // GET: ChuongTrinh/XoaHocPhan/5
        public ActionResult XoaHocPhan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuongTrinh_HocPhan chuongTrinh_HocPhan = db.ChuongTrinh_HocPhan.Find(id);
            if (chuongTrinh_HocPhan == null)
            {
                return HttpNotFound();
            }
            XoaHocPhanModel xoaHocPhanModel = new XoaHocPhanModel();
            xoaHocPhanModel.ID = chuongTrinh_HocPhan.ID;
            xoaHocPhanModel.ChuongTrinh_ID = chuongTrinh_HocPhan.ChuongTrinh_ID;
            xoaHocPhanModel.MaHP = db.HocPhan.Find(chuongTrinh_HocPhan.HocPhan_ID).MaHP;
            return View(xoaHocPhanModel);
        }

        // POST: ChuongTrinh/Delete/5
        [HttpPost, ActionName("XoaHocPhan")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaHocPhanConfirmed(int id)
        {
            ChuongTrinh_HocPhan chuongTrinh_HocPhan = db.ChuongTrinh_HocPhan.Find(id);
            db.ChuongTrinh_HocPhan.Remove(chuongTrinh_HocPhan);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = chuongTrinh_HocPhan.ChuongTrinh_ID});
        }

        // GET: ChuongTrinh/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChuongTrinh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenCT,NgayTao")] ChuongTrinh chuongTrinh)
        {
            if (ModelState.IsValid)
            {
                if (db.ChuongTrinh.Where(x => x.TenCT == chuongTrinh.TenCT).Count() > 0)
                {
                    ModelState.AddModelError("TenCT", "Tên chương trình đã tồn tại trên hệ thống!");
                    return View(chuongTrinh);
                }
                if (db.ChuongTrinh.Where(x => x.NgayTao == chuongTrinh.NgayTao).Count() > 0)
                {
                    ModelState.AddModelError("NgayTao", "Ngày tạo đã tồn tại trên hệ thống!");
                    return View(chuongTrinh);
                }
                db.ChuongTrinh.Add(chuongTrinh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chuongTrinh);
        }

        // GET: ChuongTrinh/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuongTrinh chuongTrinh = db.ChuongTrinh.Find(id);
            if (chuongTrinh == null)
            {
                return HttpNotFound();
            }
            return View(chuongTrinh);
        }

        // POST: ChuongTrinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenCT,NgayTao")] ChuongTrinh chuongTrinh)
        {
            if (ModelState.IsValid)
            {
                if (db.ChuongTrinh.Where(x => x.ID != chuongTrinh.ID && x.TenCT == chuongTrinh.TenCT).Count() > 0)
                {
                    ModelState.AddModelError("TenCT", "Tên chương trình đã tồn tại trên hệ thống!");
                    return View(chuongTrinh);
                }
                if (db.ChuongTrinh.Where(x => x.ID != chuongTrinh.ID && x.NgayTao == chuongTrinh.NgayTao).Count() > 0)
                {
                    ModelState.AddModelError("NgayTao", "Ngày tạo đã tồn tại trên hệ thống!");
                    return View(chuongTrinh);
                }

                db.Entry(chuongTrinh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chuongTrinh);
        }

        // GET: ChuongTrinh/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuongTrinh chuongTrinh = db.ChuongTrinh.Find(id);
            if (chuongTrinh == null)
            {
                return HttpNotFound();
            }
            return View(chuongTrinh);
        }

        // POST: ChuongTrinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChuongTrinh chuongTrinh = db.ChuongTrinh.Find(id);
            db.ChuongTrinh.Remove(chuongTrinh);
            db.SaveChanges();
            return RedirectToAction("Index");
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
