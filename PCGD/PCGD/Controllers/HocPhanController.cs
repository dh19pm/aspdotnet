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
    [Authentication]
    [Role("Admin")]
    public class HocPhanController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: HocPhan
        public ActionResult Index()
        {
            return View(db.HocPhan.OrderByDescending(x => x.ID).ToList());
        }

        // GET: HocPhan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HocPhan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaHP,LoaiHP,TenHP,SoTC")] HocPhan hocPhan)
        {
            if (ModelState.IsValid)
            {
                if (db.HocPhan.Where(x => x.MaHP == hocPhan.MaHP).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã tồn tại trên hệ thống!");
                    return View(hocPhan);
                }

                db.HocPhan.Add(hocPhan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hocPhan);
        }

        // GET: HocPhan/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocPhan hocPhan = db.HocPhan.Find(id);
            if (hocPhan == null)
            {
                return HttpNotFound();
            }
            return View(hocPhan);
        }

        // POST: HocPhan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaHP,TenHP,SoTC")] HocPhan hocPhan)
        {
            if (ModelState.IsValid)
            {
                if (db.HocPhan.Where(x => x.ID != hocPhan.ID && x.MaHP == hocPhan.MaHP).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã tồn tại trên hệ thống!");
                    return View(hocPhan);
                }

                HocPhan editHocPhan = db.HocPhan.Find(hocPhan.ID);
                editHocPhan.MaHP = hocPhan.MaHP;
                editHocPhan.SoTC = hocPhan.SoTC;
                editHocPhan.TenHP = hocPhan.TenHP;

                db.Entry(editHocPhan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hocPhan);
        }

        // GET: HocPhan/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocPhan hocPhan = db.HocPhan.Find(id);
            if (hocPhan == null)
            {
                return HttpNotFound();
            }
            return View(hocPhan);
        }

        // POST: HocPhan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            HocPhan hocPhan = db.HocPhan.Find(id);
            db.HocPhan.Remove(hocPhan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Post: HocPhan/Search
        public JsonResult Search(string mahp, string tengv, string tenlop)
        {
            tengv = HttpUtility.UrlDecode(tengv);
            if (!string.IsNullOrEmpty(mahp) && string.IsNullOrEmpty(tengv) && string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = db.HocPhan.Where(x => x.MaHP.Contains(mahp)).Select(x => new { value = x.MaHP }).ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (string.IsNullOrEmpty(mahp) && !string.IsNullOrEmpty(tengv) && string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from g in db.GiangVien
                            join c in db.ChiTietGiangVien on g.ID equals c.GiangVien_ID
                            join h in db.HocPhan on c.HocPhan_ID equals h.ID
                            where g.TenGV == tengv
                            select new { value = h.MaHP }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (string.IsNullOrEmpty(mahp) && string.IsNullOrEmpty(tengv) && !string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from h in db.HocPhan
                            join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                            join n in db.NhomHocPhan on c.NhomHocPhan_ID equals n.ID
                            join k in db.HocKi on n.HocKi_ID equals k.ID
                            join t in db.ChuongTrinh on k.ChuongTrinh_ID equals t.ID
                            join l in db.Lop on t.ID equals l.ChuongTrinh_ID
                            where l.TenLop == tenlop
                            select new { value = h.MaHP }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (!string.IsNullOrEmpty(mahp) && !string.IsNullOrEmpty(tengv) && string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from g in db.GiangVien
                            join c in db.ChiTietGiangVien on g.ID equals c.GiangVien_ID
                            join h in db.HocPhan on c.HocPhan_ID equals h.ID
                            where h.MaHP.Contains(mahp) && g.TenGV == tengv
                            select new { value = h.MaHP }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (!string.IsNullOrEmpty(mahp) && string.IsNullOrEmpty(tengv) && !string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from h in db.HocPhan
                            join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                            join n in db.NhomHocPhan on c.NhomHocPhan_ID equals n.ID
                            join k in db.HocKi on n.HocKi_ID equals k.ID
                            join t in db.ChuongTrinh on k.ChuongTrinh_ID equals t.ID
                            join l in db.Lop on t.ID equals l.ChuongTrinh_ID
                            where h.MaHP.Contains(mahp) && l.TenLop == tenlop
                            select new { value = h.MaHP }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (string.IsNullOrEmpty(mahp) && !string.IsNullOrEmpty(tengv) && !string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from h in db.HocPhan
                            join v in db.ChiTietGiangVien on h.ID equals v.HocPhan_ID
                            join g in db.GiangVien on v.GiangVien_ID equals g.ID
                            join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                            join n in db.NhomHocPhan on c.NhomHocPhan_ID equals n.ID
                            join k in db.HocKi on n.HocKi_ID equals k.ID
                            join t in db.ChuongTrinh on k.ChuongTrinh_ID equals t.ID
                            join l in db.Lop on t.ID equals l.ChuongTrinh_ID
                            where l.TenLop == tenlop && g.TenGV == tengv
                            select new { value = h.MaHP }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (!string.IsNullOrEmpty(mahp) && !string.IsNullOrEmpty(tengv) && !string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from h in db.HocPhan
                            join v in db.ChiTietGiangVien on h.ID equals v.HocPhan_ID
                            join g in db.GiangVien on v.GiangVien_ID equals g.ID
                            join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                            join n in db.NhomHocPhan on c.NhomHocPhan_ID equals n.ID
                            join k in db.HocKi on n.HocKi_ID equals k.ID
                            join t in db.ChuongTrinh on k.ChuongTrinh_ID equals t.ID
                            join l in db.Lop on t.ID equals l.ChuongTrinh_ID
                            where h.MaHP.Contains(mahp) && l.TenLop == tenlop && g.TenGV == tengv
                            select new { value = h.MaHP }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = db.HocPhan.Select(x => new { value = x.MaHP }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
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
