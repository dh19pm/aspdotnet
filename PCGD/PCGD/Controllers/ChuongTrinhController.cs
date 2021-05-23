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
    [Role("Admin")]
    public class ChuongTrinhController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: ChuongTrinh
        public ActionResult Index()
        {
            return View(db.ChuongTrinh.OrderBy(x => x.NgayTao).ToList());
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
            ViewChuongTrinhModel viewChuongTrinhModel = new ViewChuongTrinhModel();
            viewChuongTrinhModel.ChuongTrinh = chuongTrinh;
            viewChuongTrinhModel.HocKi = HocPhanLib.GetViewHocKiModel();
            viewChuongTrinhModel.NhomHocPhan = db.NhomHocPhan;
            viewChuongTrinhModel.ChiTietHocPhan = db.ChiTietHocPhan;
            viewChuongTrinhModel.HocPhan = db.HocPhan;
            return View(viewChuongTrinhModel);
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
                db.ChuongTrinh.Add(chuongTrinh);
                db.SaveChanges();

                for (byte i = 1; i <= 8; i++)
                {
                    HocKi hocKi = new HocKi();
                    hocKi.ChuongTrinh_ID = chuongTrinh.ID;
                    hocKi.SoHocKi = i;
                    db.HocKi.Add(hocKi);
                    db.SaveChanges();
                }

                return RedirectToAction("Details", new { id = chuongTrinh.ID });
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
            if (chuongTrinh == null)
            {
                return HttpNotFound();
            }
            db.ChuongTrinh.Remove(chuongTrinh);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: ChuongTrinh/ThemNhomHocPhan/HocKi_ID
        public ActionResult ThemNhomHocPhan(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocKi hocKi = db.HocKi.Find(id);
            if (hocKi == null)
            {
                return HttpNotFound();
            }
            NhomHocPhan nhomHocPhan = new NhomHocPhan();
            nhomHocPhan.HocKi_ID = Convert.ToInt64(id);
            nhomHocPhan.TongTC = 0;
            db.NhomHocPhan.Add(nhomHocPhan);
            db.SaveChanges();
            return RedirectToAction("NhomHocPhan", "ChuongTrinh", new { id = nhomHocPhan.ID });
        }

        // GET: ChuongTrinh/NhomHocPhan/HocKi_ID/NhomHocPhan_ID
        public ActionResult NhomHocPhan(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomHocPhan nhomHocPhan = db.NhomHocPhan.Find(id);
            if (nhomHocPhan == null)
            {
                return HttpNotFound();
            }
            HocKi hocKi = db.HocKi.Find(nhomHocPhan.HocKi_ID);
            if (hocKi == null)
            {
                return HttpNotFound();
            }
            ChuongTrinh chuongTrinh = db.ChuongTrinh.Find(hocKi.ChuongTrinh_ID);
            ViewNhomHocPhan viewNhomHocPhan = new ViewNhomHocPhan();
            NhomHocPhanModel nhomHocPhanModel = new NhomHocPhanModel();
            nhomHocPhanModel.NhomHocPhan_ID = nhomHocPhan.ID;
            nhomHocPhanModel.TongTC = nhomHocPhan.TongTC;
            nhomHocPhanModel.HocPhanDieuKien = nhomHocPhan.HocPhanDieuKien;
            nhomHocPhanModel.HocPhanThayThe = nhomHocPhan.HocPhanThayThe;
            List<HocPhanModel> hocPhanModel = HocPhanLib.GetHocPhanModel(Convert.ToInt64(id));
            viewNhomHocPhan.ChuongTrinh = chuongTrinh;
            viewNhomHocPhan.NhomHocPhan = nhomHocPhan;
            viewNhomHocPhan.HocPhan = hocPhanModel;
            viewNhomHocPhan.NhomHocPhanModel = nhomHocPhanModel;
            return View(viewNhomHocPhan);
        }

        // POST: ChuongTrinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NhomHocPhan([Bind(Include = "NhomHocPhan_ID,HocPhanDieuKien,HocPhanThayThe,TongTC")] NhomHocPhanModel nhomHocPhanModel)
        {
            if (ModelState.IsValid)
            {
                NhomHocPhan nhomHocPhan = db.NhomHocPhan.Find(nhomHocPhanModel.NhomHocPhan_ID);
                if (nhomHocPhan == null)
                    return HttpNotFound();
                if (HocPhanLib.GetCountHocPhan(nhomHocPhan.ID) >= 2 && HocPhanLib.GetCountHocPhan(nhomHocPhan.ID) == HocPhanLib.GetCountHocPhanTuChon(nhomHocPhan.ID))
                    nhomHocPhan.TongTC = nhomHocPhanModel.TongTC;
                else
                    nhomHocPhan.TongTC = 0;
                nhomHocPhan.HocPhanDieuKien = nhomHocPhanModel.HocPhanDieuKien;
                nhomHocPhan.HocPhanThayThe = nhomHocPhanModel.HocPhanThayThe;
                db.Entry(nhomHocPhan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ChuongTrinh", new { id = db.HocKi.Find(nhomHocPhan.HocKi_ID).ChuongTrinh_ID });
            }
            return View(nhomHocPhanModel);
        }

        // GET: ChuongTrinh/ThemHocPhan/HocKi_ID/NhomHocPhan_ID
        public ActionResult ThemHocPhan(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomHocPhan nhomHocPhan = db.NhomHocPhan.Find(id);
            if (nhomHocPhan == null)
            {
                return HttpNotFound();
            }
            ThemHocPhanModel themHocPhanModel = new ThemHocPhanModel();
            themHocPhanModel.NhomHocPhan_ID = nhomHocPhan.ID;
            return View(themHocPhanModel);
        }

        // POST: ChuongTrinh/ThemHocPhan
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemHocPhan([Bind(Include = "NhomHocPhan_ID,MaHP,SoTietLT,SoTietTH")] ThemHocPhanModel themHocPhanModel)
        {
            if (ModelState.IsValid)
            {
                HocPhan hocPhan = db.HocPhan.Where(x => x.MaHP == themHocPhanModel.MaHP).FirstOrDefault();
                if (hocPhan == null)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần không tồn tại trên hệ thống!");
                    return View(themHocPhanModel);
                }
                if (HocPhanLib.ExistsMaHocPhan(themHocPhanModel.NhomHocPhan_ID, (string)hocPhan.MaHP))
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã đã tồn tại!");
                    return View(themHocPhanModel);
                }
                if (HocPhanLib.GetCountHocPhan(themHocPhanModel.NhomHocPhan_ID) > 0 && HocPhanLib.GetCountHocPhan(themHocPhanModel.NhomHocPhan_ID) == HocPhanLib.GetCountHocPhanTuChon(themHocPhanModel.NhomHocPhan_ID) && hocPhan.LoaiHP == 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần bắt buộc không thể nhóm với học phần tự chọn!");
                    return View(themHocPhanModel);
                }
                if (HocPhanLib.GetCountHocPhan(themHocPhanModel.NhomHocPhan_ID) > 0 && HocPhanLib.GetCountHocPhan(themHocPhanModel.NhomHocPhan_ID) == HocPhanLib.GetCountHocPhanBatBuoc(themHocPhanModel.NhomHocPhan_ID) && hocPhan.LoaiHP == 1)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần tự chọn không thể nhóm với học phần bắt buộc!");
                    return View(themHocPhanModel);
                }
                if (HocPhanLib.GetCountHocPhanBatBuoc(themHocPhanModel.NhomHocPhan_ID) > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần bắt buộc không thể nhóm với nhau!");
                    return View(themHocPhanModel);
                }
                ChiTietHocPhan chiTietHocPhan = new ChiTietHocPhan();
                chiTietHocPhan.NhomHocPhan_ID = themHocPhanModel.NhomHocPhan_ID;
                chiTietHocPhan.HocPhan_ID = hocPhan.ID;
                chiTietHocPhan.SoTietLT = themHocPhanModel.SoTietLT;
                chiTietHocPhan.SoTietTH = themHocPhanModel.SoTietTH;
                db.ChiTietHocPhan.Add(chiTietHocPhan);
                db.SaveChanges();
                return RedirectToAction("NhomHocPhan", "ChuongTrinh", new { id = themHocPhanModel.NhomHocPhan_ID });
            }

            return View(themHocPhanModel);
        }

        // GET: ChuongTrinh/SuaHocPhan/ChiTietHocPhan_ID
        public ActionResult SuaHocPhan(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietHocPhan chiTietHocPhan = db.ChiTietHocPhan.Find(id);
            if (chiTietHocPhan == null)
            {
                return HttpNotFound();
            }
            NhomHocPhan nhomHocPhan = db.NhomHocPhan.Find(chiTietHocPhan.NhomHocPhan_ID);
            if (nhomHocPhan == null)
            {
                return HttpNotFound();
            }
            SuaHocPhanModel suaHocPhanModel = new SuaHocPhanModel();
            suaHocPhanModel.NhomHocPhan_ID = nhomHocPhan.ID;
            suaHocPhanModel.ChiTietHocPhan_ID = chiTietHocPhan.ID;
            suaHocPhanModel.MaHP = db.HocPhan.Find(chiTietHocPhan.HocPhan_ID).MaHP;
            suaHocPhanModel.SoTietLT = chiTietHocPhan.SoTietLT;
            suaHocPhanModel.SoTietTH = chiTietHocPhan.SoTietTH;
            return View(suaHocPhanModel);
        }

        // POST: ChuongTrinh/SuaHocPhan/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaHocPhan([Bind(Include = "HocKi_ID,NhomHocPhan_ID,ChiTietHocPhan_ID,MaHP,SoTietLT,SoTietTH")] SuaHocPhanModel suaHocPhanModel)
        {
            if (ModelState.IsValid)
            {
                HocPhan hocPhan = db.HocPhan.Where(x => x.MaHP == suaHocPhanModel.MaHP).FirstOrDefault();
                if (hocPhan == null)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần không tồn tại trên hệ thống!");
                    return View(suaHocPhanModel);
                }
                if (HocPhanLib.ExistsMaHocPhan(suaHocPhanModel.NhomHocPhan_ID, (string)hocPhan.MaHP, suaHocPhanModel.ChiTietHocPhan_ID))
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã đã tồn tại!");
                    return View(suaHocPhanModel);
                }
                if (HocPhanLib.GetCountHocPhan(suaHocPhanModel.NhomHocPhan_ID) > 1 && HocPhanLib.GetCountHocPhan(suaHocPhanModel.NhomHocPhan_ID) == HocPhanLib.GetCountHocPhanTuChon(suaHocPhanModel.NhomHocPhan_ID) && hocPhan.LoaiHP == 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần bắt buộc không thể nhóm với học phần tự chọn!");
                    return View(suaHocPhanModel);
                }
                if (HocPhanLib.GetCountHocPhan(suaHocPhanModel.NhomHocPhan_ID) > 1 && HocPhanLib.GetCountHocPhan(suaHocPhanModel.NhomHocPhan_ID) == HocPhanLib.GetCountHocPhanBatBuoc(suaHocPhanModel.NhomHocPhan_ID) && hocPhan.LoaiHP == 1)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần tự chọn không thể nhóm với học phần bắt buộc!");
                    return View(suaHocPhanModel);
                }
                if (HocPhanLib.GetCountHocPhanBatBuoc(suaHocPhanModel.NhomHocPhan_ID) > 1)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần bắt buộc không thể nhóm với nhau!");
                    return View(suaHocPhanModel);
                }

                ChiTietHocPhan chiTietHocPhan = new ChiTietHocPhan();
                chiTietHocPhan.ID = suaHocPhanModel.ChiTietHocPhan_ID;
                chiTietHocPhan.HocPhan_ID = hocPhan.ID;
                chiTietHocPhan.NhomHocPhan_ID = suaHocPhanModel.NhomHocPhan_ID;
                chiTietHocPhan.SoTietLT = suaHocPhanModel.SoTietLT;
                chiTietHocPhan.SoTietTH = suaHocPhanModel.SoTietTH;

                db.Entry(chiTietHocPhan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NhomHocPhan", "ChuongTrinh", new { id = suaHocPhanModel.NhomHocPhan_ID });
            }
            return View(suaHocPhanModel);
        }

        // GET: ChuongTrinh/XoaHocPhan/
        public ActionResult XoaHocPhan(long? id)
        {
            XoaHocPhanModel xoaHocPhanModel = new XoaHocPhanModel();
            ChiTietHocPhan chiTietHocPhan = db.ChiTietHocPhan.Find(id);

            if (chiTietHocPhan == null)
            {
                return HttpNotFound();
            }

            NhomHocPhan nhomHocPhan = db.NhomHocPhan.Find(chiTietHocPhan.NhomHocPhan_ID);
            xoaHocPhanModel.MaHP = db.HocPhan.Find(chiTietHocPhan.HocPhan_ID).MaHP;
            xoaHocPhanModel.NhomHocPhan_ID = nhomHocPhan.ID;
            return View(xoaHocPhanModel);
        }

        // POST: ChuongTrinh/Delete/5
        [HttpPost, ActionName("XoaHocPhan")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaHocPhanConfirm(long? id)
        {
            ChiTietHocPhan chiTietHocPhan = db.ChiTietHocPhan.Find(id);
            if (chiTietHocPhan == null)
            {
                return HttpNotFound();
            }
            db.ChiTietHocPhan.Remove(chiTietHocPhan);
            db.SaveChanges();
            NhomHocPhan nhomHocPhan = db.NhomHocPhan.Find(chiTietHocPhan.NhomHocPhan_ID);
            if (nhomHocPhan == null)
            {
                return HttpNotFound();
            }
            if (HocPhanLib.GetCountHocPhan(chiTietHocPhan.NhomHocPhan_ID) <= 1)
            {
                if (HocPhanLib.GetCountHocPhan(chiTietHocPhan.NhomHocPhan_ID) <= 0)
                {
                    nhomHocPhan.HocPhanDieuKien = 0;
                }
                nhomHocPhan.TongTC = 0;
                db.Entry(nhomHocPhan).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("NhomHocPhan", "ChuongTrinh", new { id = nhomHocPhan.ID });
        }

        // GET: ChuongTrinh/XoaNhomHocPhan/
        public ActionResult XoaNhomHocPhan(long? id)
        {
            NhomHocPhan nhomHocPhan = db.NhomHocPhan.Find(id);
            if (nhomHocPhan == null)
            {
                return HttpNotFound();
            }
            List<HocPhanModel> hocPhanModel = HocPhanLib.GetHocPhanModel(Convert.ToInt64(id));
            XoaNhomHocPhanModel xoaNhomHocPhanModel = new XoaNhomHocPhanModel();
            xoaNhomHocPhanModel.HocPhan = hocPhanModel;
            xoaNhomHocPhanModel.ChuongTrinh_ID = db.HocKi.Find(nhomHocPhan.HocKi_ID).ChuongTrinh_ID;
            return View(xoaNhomHocPhanModel);
        }

        // POST: ChuongTrinh/Delete/5
        [HttpPost, ActionName("XoaNhomHocPhan")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaNhomHocPhanConfirm(long? id)
        {
            NhomHocPhan nhomHocPhan = db.NhomHocPhan.Find(id);
            if (nhomHocPhan == null)
            {
                return HttpNotFound();
            }
            db.NhomHocPhan.Remove(nhomHocPhan);
            db.SaveChanges();
            return RedirectToAction("Details", "ChuongTrinh", new { id = db.HocKi.Find(nhomHocPhan.HocKi_ID).ChuongTrinh_ID });
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
