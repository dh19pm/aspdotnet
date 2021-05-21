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
            return View(phanCong.OrderByDescending(x => x.NamHoc).ToList());
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
            ViewNhiemVuModel viewNhiemVuModel = new ViewNhiemVuModel();
            viewNhiemVuModel.PhanCong = phanCong;
            viewNhiemVuModel.Lop = PhanCongLib.GetNhiemVuLopModel(Convert.ToInt64(id));
            viewNhiemVuModel.NhomHocPhan = PhanCongLib.GetNhiemVuNhomHocPhanModel(Convert.ToInt64(id));
            viewNhiemVuModel.NhiemVu = PhanCongLib.GetNhiemVuModel(Convert.ToInt64(id));
            return View(viewNhiemVuModel);
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
        public ActionResult Create([Bind(Include = "ID,NamHoc,HocKi")] PhanCong phanCong)
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
        public ActionResult Edit([Bind(Include = "ID,NguoiDung_ID,NamHoc,HocKi")] PhanCong phanCong)
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
        public ActionResult ThemNhiemVu([Bind(Include = "PhanCong_ID,TenGV,MaHP,TenLop,LoaiPhong,NhomLT,NhomTH,GhiChu")] ThemNhiemVuModel themNhiemVuModel)
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
                    ModelState.AddModelError("TenLop", "Chương trình của tên lớp này không có học phần \"" + hocPhan.MaHP + "\"");
                    return View(themNhiemVuModel);
                }
                nhiemVu.Lop_ID = lop.ID;
                nhiemVu.LoaiPhong = themNhiemVuModel.LoaiPhong;
                nhiemVu.NhomLT = themNhiemVuModel.NhomLT;
                nhiemVu.NhomTH = themNhiemVuModel.NhomTH;
                nhiemVu.GhiChu = themNhiemVuModel.GhiChu;
                db.NhiemVu.Add(nhiemVu);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = themNhiemVuModel.PhanCong_ID });
            }

            return View(themNhiemVuModel);
        }

        // GET: PhanCong/Edit/5
        public ActionResult SuaNhiemVu(long? nhiemvu_id)
        {
            if (nhiemvu_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhiemVu nhiemVu = db.NhiemVu.Find(nhiemvu_id);
            if (nhiemVu == null)
            {
                return HttpNotFound();
            }
            SuaNhiemVuModel suaNhiemVuModel = new SuaNhiemVuModel();
            suaNhiemVuModel.ID = nhiemVu.ID;
            suaNhiemVuModel.PhanCong_ID = nhiemVu.PhanCong_ID;
            GiangVien giangVien = db.GiangVien.Find(nhiemVu.GiangVien_ID);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            suaNhiemVuModel.TenGV = giangVien.TenGV;
            HocPhan hocPhan = db.HocPhan.Find(nhiemVu.HocPhan_ID);
            if (hocPhan == null)
            {
                return HttpNotFound();
            }
            suaNhiemVuModel.MaHP = hocPhan.MaHP;
            Lop lop = db.Lop.Find(nhiemVu.Lop_ID);
            if (lop == null)
            {
                return HttpNotFound();
            }
            suaNhiemVuModel.TenLop = lop.TenLop;
            suaNhiemVuModel.LoaiPhong = nhiemVu.LoaiPhong;
            suaNhiemVuModel.NhomLT = nhiemVu.NhomLT;
            suaNhiemVuModel.NhomTH = nhiemVu.NhomTH;
            suaNhiemVuModel.GhiChu = nhiemVu.GhiChu;
            return View(suaNhiemVuModel);
        }

        // POST: PhanCong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaNhiemVu([Bind(Include = "ID,PhanCong_ID,TenGV,MaHP,TenLop,LoaiPhong,NhomLT,NhomTH,GhiChu")] SuaNhiemVuModel suaNhiemVuModel)
        {
            if (ModelState.IsValid)
            {
                NhiemVu nhiemVu = new NhiemVu();
                nhiemVu.ID = suaNhiemVuModel.ID;
                nhiemVu.PhanCong_ID = suaNhiemVuModel.PhanCong_ID;
                GiangVien giangVien = db.GiangVien.Where(x => x.TenGV == suaNhiemVuModel.TenGV).SingleOrDefault();
                if (giangVien == null)
                {
                    ModelState.AddModelError("TenGV", "Tên giảng viên không tồn tại trên hệ thống!");
                    return View(suaNhiemVuModel);
                }
                nhiemVu.GiangVien_ID = giangVien.ID;
                HocPhan hocPhan = db.HocPhan.Where(x => x.MaHP == suaNhiemVuModel.MaHP).SingleOrDefault();
                if (hocPhan == null)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần không tồn tại trên hệ thống!");
                    return View(suaNhiemVuModel);
                }
                if (!PhanCongLib.IsHocPhanOfGiangVien(giangVien.TenGV, hocPhan.MaHP))
                {
                    ModelState.AddModelError("MaHP", "Mã học phần này giảng viên \"" + giangVien.TenGV + "\" không có dạy");
                    return View(suaNhiemVuModel);
                }
                nhiemVu.HocPhan_ID = hocPhan.ID;
                Lop lop = db.Lop.Where(x => x.TenLop == suaNhiemVuModel.TenLop).SingleOrDefault();
                if (lop == null)
                {
                    ModelState.AddModelError("TenLop", "Tên lớp không tồn tại trên hệ thống!");
                    return View(suaNhiemVuModel);
                }
                if (!PhanCongLib.IsLopOfHocPhan(hocPhan.MaHP, lop.TenLop))
                {
                    ModelState.AddModelError("TenLop", "Chương trình của tên lớp này không có học phần \"" + hocPhan.MaHP + "\"");
                    return View(suaNhiemVuModel);
                }
                nhiemVu.Lop_ID = lop.ID;
                nhiemVu.LoaiPhong = suaNhiemVuModel.LoaiPhong;
                nhiemVu.NhomLT = suaNhiemVuModel.NhomLT;
                nhiemVu.NhomTH = suaNhiemVuModel.NhomTH;
                nhiemVu.GhiChu = suaNhiemVuModel.GhiChu;
                db.Entry(nhiemVu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = suaNhiemVuModel.PhanCong_ID });
            }

            return View(suaNhiemVuModel);
        }

        // GET: PhanCong/Delete/5
        public ActionResult XoaNhiemVu(long? nhiemvu_id)
        {
            if (nhiemvu_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhiemVu nhiemVu = db.NhiemVu.Find(nhiemvu_id);
            if (nhiemVu == null)
            {
                return HttpNotFound();
            }
            XoaNhiemVuModel xoaNhiemVuModel = new XoaNhiemVuModel();
            xoaNhiemVuModel.ID = nhiemVu.ID;
            xoaNhiemVuModel.PhanCong_ID = nhiemVu.PhanCong_ID;
            xoaNhiemVuModel.TenGV = nhiemVu.GiangVien.TenGV;
            return View(xoaNhiemVuModel);
        }

        // POST: PhanCong/Delete/5
        [HttpPost, ActionName("XoaNhiemVu")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaNhiemVuConfirmed(long nhiemvu_id)
        {
            NhiemVu nhiemVu = db.NhiemVu.Find(nhiemvu_id);
            db.NhiemVu.Remove(nhiemVu);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = nhiemVu.PhanCong_ID });
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
